package com.example.paymentDemo.integration;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.event.PaymentInitiatedEvent;
import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.RefundIssuedEvent;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.repository.PaymentRepository;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import static org.assertj.core.api.Assertions.assertThat;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;


@SpringBootTest
@AutoConfigureMockMvc
public class PaymentControllerIntegrationTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private PaymentRepository paymentRepository;

    @Autowired
    private ObjectMapper objectMapper;

    @MockBean
    private RabbitTemplate rabbitTemplate;

    @AfterEach
    void cleanUp() {
        paymentRepository.deleteAll();
    }

    @Test
    void shouldInitiatePaymentAndPublishEvent() throws Exception {
        // Arrange
        PaymentRequest request = new PaymentRequest("123", 150.0, "credit_card", "Billing Info", 9999L);
        String json = objectMapper.writeValueAsString(request);

        // Act & Assert
        mockMvc.perform(post("/payment/initiate")
                .contentType(MediaType.APPLICATION_JSON)
                .content(json))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$.status").value(PaymentStatus.PENDING.name()))
            .andExpect(jsonPath("$.amount").value(150.0))
            .andExpect(jsonPath("$.method").value("credit_card"))
            .andExpect(jsonPath("$.appointmentId").value(9999));

        // DB’de bir kayıt oluştu mu?
        assertThat(paymentRepository.findAll())
            .hasSize(1)
            .first()
            .extracting("status", "userId", "appointmentId")
            .containsExactly(PaymentStatus.PENDING, "123", 9999L);

        verify(rabbitTemplate).convertAndSend(
            eq("payment.exchange"), eq("payment.initiated"), 
            any(PaymentInitiatedEvent.class));
    }

    @Test
    void shouldConfirmPaymentAndPublishSucceededEvent() throws Exception {
        // Prepare a PENDING payment in DB
         Payment p = new Payment();
        p.setUserId("999");
        p.setAppointmentId(1111L);
        p.setAmount(50.0);
        p.setMethod("card");
        p.setBillingDetails("x");
        p.setStatus(PaymentStatus.PENDING);
        p = paymentRepository.save(p);

        // Send confirm request
        String json = objectMapper.writeValueAsString(
        new PaymentResult(p.getId(), true, "TXN-123", null)
    );

        mockMvc.perform(post("/payment/confirm")
                .contentType(MediaType.APPLICATION_JSON)
                .content(json))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$.status").value(PaymentStatus.COMPLETED.name()))
            .andExpect(jsonPath("$.id").value(p.getId().intValue()))
            .andExpect(jsonPath("$.amount").value(p.getAmount()));

        // DB updated?
        Payment updated = paymentRepository.findById(p.getId()).get();
        assertThat(updated.getStatus()).isEqualTo(PaymentStatus.COMPLETED);

        verify(rabbitTemplate).convertAndSend(
            eq("payment.exchange"),
            eq("payment.succeeded"),
            any(PaymentSucceededEvent.class)
        );
    }

    @Test
    void shouldConfirmPaymentAndPublishFailedEvent() throws Exception {
    // Arrange: PENDING durumda bir ödeme oluştur
    Payment p = new Payment();
    p.setUserId("999");
    p.setAppointmentId(1111L);
    p.setAmount(50.0);
    p.setMethod("card");
    p.setBillingDetails("x");
    p.setStatus(PaymentStatus.PENDING);
    p = paymentRepository.save(p);

    // Confirm request (ödemenin başarısız olduğunu simüle et)
    PaymentResult result = new PaymentResult(p.getId(), false, "TXN-123", "Insufficient Funds");
    String json = objectMapper.writeValueAsString(result);

    // Act & Assert: Confirm ödeme isteği gönder
    mockMvc.perform(post("/payment/confirm")
            .contentType(MediaType.APPLICATION_JSON)
            .content(json))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.status").value(PaymentStatus.FAILED.name()));

    // DB güncellenmiş mi?
    Payment updated = paymentRepository.findById(p.getId()).get();
    assertThat(updated.getStatus()).isEqualTo(PaymentStatus.FAILED);

    // RabbitMQ event yayınlandı mı?
    verify(rabbitTemplate).convertAndSend(
        eq("payment.exchange"),
        eq("payment.failed"),
        any(PaymentFailedEvent.class)
    );
}

    @Test
    void shouldRetryPaymentAndPublishInitiatedEvent() throws Exception {
    // Arrange: FAILED durumda bir ödeme oluştur
    Payment p = new Payment();
    p.setUserId("999");
    p.setAppointmentId(1111L);
    p.setAmount(50.0);
    p.setMethod("card");
    p.setBillingDetails("x");
    p.setStatus(PaymentStatus.FAILED);
    p = paymentRepository.save(p);

    // Retry request gönder
    String json = objectMapper.writeValueAsString(p);

    // Act & Assert: Retry ödeme isteği gönder
    mockMvc.perform(post("/payment/retry/" + p.getId())
            .contentType(MediaType.APPLICATION_JSON)
            .content(json))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.status").value(PaymentStatus.PENDING.name()));

    // DB güncellenmiş mi?
    Payment updated = paymentRepository.findById(p.getId()).get();
    assertThat(updated.getStatus()).isEqualTo(PaymentStatus.PENDING);

    // RabbitMQ event yayınlandı mı?
    verify(rabbitTemplate).convertAndSend(
        eq("payment.exchange"),
        eq("payment.initiated"),
        any(PaymentInitiatedEvent.class)
    );
}

    @Test
    void shouldIssueRefundAndPublishRefundEvent() throws Exception {
    // Arrange: COMPLETED durumda bir ödeme oluştur
    Payment p = new Payment();
    p.setUserId("999");
    p.setAppointmentId(1111L);
    p.setAmount(50.0);
    p.setMethod("card");
    p.setBillingDetails("x");
    p.setStatus(PaymentStatus.COMPLETED);
    p = paymentRepository.save(p);

    // Refund request gönder
    String json = objectMapper.writeValueAsString(p);

    // Act & Assert: Refund ödeme isteği gönder
    mockMvc.perform(post("/payment/refund/" + p.getId())
            .contentType(MediaType.APPLICATION_JSON)
            .content(json))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$.status").value(PaymentStatus.REFUNDED.name()));

    // DB güncellenmiş mi?
    Payment updated = paymentRepository.findById(p.getId()).get();
    assertThat(updated.getStatus()).isEqualTo(PaymentStatus.REFUNDED);

    // RabbitMQ event yayınlandı mı?
    verify(rabbitTemplate).convertAndSend(
        eq("payment.exchange"),
        eq("payment.refunded"),
        any(RefundIssuedEvent.class)
    );
}


    @Test
    void shouldGetPaymentStatus() throws Exception {
    // Arrange: COMPLETED durumda bir ödeme oluştur
    Payment p = new Payment();
    p.setUserId("999");
    p.setAppointmentId(1111L);
    p.setAmount(50.0);
    p.setMethod("card");
    p.setBillingDetails("cibili cibili şak şak");
    p.setStatus(PaymentStatus.COMPLETED);
    p = paymentRepository.save(p);

    // Act & Assert: Ödeme durumunu al
    mockMvc.perform(get("/payment/status/" + p.getId())
            .contentType(MediaType.APPLICATION_JSON))
        .andExpect(status().isOk())
        .andExpect(jsonPath("$").value(PaymentStatus.COMPLETED.name()));

    // DB'deki ödeme durumu kontrol edilsin mi?
    Payment fetched = paymentRepository.findById(p.getId()).get();
    assertThat(fetched.getStatus()).isEqualTo(PaymentStatus.COMPLETED);
}


    
}
