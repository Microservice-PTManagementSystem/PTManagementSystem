/*package com.example.paymentDemo.integration;

import static org.assertj.core.api.Assertions.assertThat;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;
import org.springframework.transaction.annotation.Transactional;

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

@SpringBootTest
@AutoConfigureMockMvc
@Transactional
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
        PaymentRequest request = new PaymentRequest("credit card", "1234567890123456",
            "John Doe", "12", "2025", "123", "100.00");
        String json = objectMapper.writeValueAsString(request);

        // Act & Assert
        mockMvc.perform(post("/payment/initiate")
                .contentType(MediaType.APPLICATION_JSON)
                .content(json))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$.status").value(PaymentStatus.PENDING.name()))
            .andExpect(jsonPath("$.paymentMethod").value("credit card"))
            .andExpect(jsonPath("$.totalAmount").value("100.00"));

        // Verify DB record
        assertThat(paymentRepository.findAll())
            .hasSize(1)
            .first()
            .extracting("status", "paymentMethod", "totalAmount")
            .containsExactly(PaymentStatus.PENDING, "credit card", "100.00");

        verify(rabbitTemplate).convertAndSend(
            eq("payment.exchange"), 
            eq("payment.initiated"), 
            any(PaymentInitiatedEvent.class)
        );
    }

    @Test
    void shouldConfirmPaymentAndPublishSucceededEvent() throws Exception {
        // Prepare a PENDING payment in DB
        Payment p = new Payment();
        p.setPaymentMethod("credit card");
        p.setCardNumber("1234567890123456");
        p.setCardHolder("John Doe");
        p.setExpiryMonth("12");
        p.setExpiryYear("2025");
        p.setCvc("123");
        p.setSaveCard(false);
        p.setTotalAmount("100.00");
        p.setStatus(PaymentStatus.PENDING);
        p = paymentRepository.save(p);

        // Create payment result with success
        PaymentResult result = new PaymentResult();
        result.setSuccess(true);
        String json = objectMapper.writeValueAsString(result);

        // Act & Assert
        mockMvc.perform(post("/payment/confirm")
                .contentType(MediaType.APPLICATION_JSON)
                .content(json))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$.success").value(true))
            .andExpect(jsonPath("$.message").value("Payment succeeded"));

        // Verify DB updated
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
        // Arrange: Create a PENDING payment
        Payment p = new Payment();
        p.setPaymentMethod("credit card");
        p.setCardNumber("1234567890123456");
        p.setCardHolder("John Doe");
        p.setExpiryMonth("12");
        p.setExpiryYear("2025");
        p.setCvc("123");
        p.setSaveCard(false);
        p.setTotalAmount("100.00");
        p.setStatus(PaymentStatus.PENDING);
        p = paymentRepository.save(p);

        // Create payment result with failure
        PaymentResult result = new PaymentResult();
        result.setSlotId(p.getSlotId());
        result.setSuccess(false);
        String json = objectMapper.writeValueAsString(result);

        // Act & Assert
        mockMvc.perform(post("/payment/confirm")
                .contentType(MediaType.APPLICATION_JSON)
                .content(json))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$.success").value(false))
            .andExpect(jsonPath("$.message").value("Payment failed"));

        // Verify DB updated
        Payment updated = paymentRepository.findBySlotId(p.getSlotId()).get();
        assertThat(updated.getStatus()).isEqualTo(PaymentStatus.FAILED);

        verify(rabbitTemplate).convertAndSend(
            eq("payment.exchange"),
            eq("payment.failed"),
            any(PaymentFailedEvent.class)
        );
    }

    @Test
    void shouldRetryPaymentAndPublishInitiatedEvent() throws Exception {
        // Arrange: Create a FAILED payment
        Payment p = new Payment();
        p.setPaymentMethod("credit card");
        p.setCardNumber("1234567890123456");
        p.setCardHolder("John Doe");
        p.setExpiryMonth("12");
        p.setExpiryYear("2025");
        p.setCvc("123");
        p.setSaveCard(false);
        p.setTotalAmount("100.00");
        p.setStatus(PaymentStatus.FAILED);
        p = paymentRepository.save(p);

        // Act & Assert
        mockMvc.perform(post("/payment/retry/" + p.getId())
                .contentType(MediaType.APPLICATION_JSON))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$.status").value(PaymentStatus.PENDING.name()));

        // Verify DB updated
        Payment updated = paymentRepository.findBySlotId(p.getId()).get();
        assertThat(updated.getStatus()).isEqualTo(PaymentStatus.PENDING);

        verify(rabbitTemplate).convertAndSend(
            eq("payment.exchange"),
            eq("payment.initiated"),
            any(PaymentInitiatedEvent.class)
        );
    }

    @Test
    void shouldIssueRefundAndPublishRefundEvent() throws Exception {
        // Arrange: Create a COMPLETED payment
        Payment p = new Payment();
        p.setPaymentMethod("credit card");
        p.setCardNumber("1234567890123456");
        p.setCardHolder("John Doe");
        p.setExpiryMonth("12");
        p.setExpiryYear("2025");
        p.setCvc("123");
        p.setSaveCard(false);
        p.setTotalAmount("100.00");
        p.setStatus(PaymentStatus.COMPLETED);
        p = paymentRepository.save(p);

        // Act & Assert
        mockMvc.perform(post("/payment/refund/" + p.getId())
                .contentType(MediaType.APPLICATION_JSON))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$.status").value(PaymentStatus.REFUNDED.name()));

        // Verify DB updated
        Payment updated = paymentRepository.findById(p.getId()).get();
        assertThat(updated.getStatus()).isEqualTo(PaymentStatus.REFUNDED);

        verify(rabbitTemplate).convertAndSend(
            eq("payment.exchange"),
            eq("payment.refunded"),
            any(RefundIssuedEvent.class)
        );
    }

    @Test
    void shouldGetPaymentStatus() throws Exception {
        // Arrange: Create a COMPLETED payment
        Payment p = new Payment();
        p.setPaymentMethod("credit card");
        p.setCardNumber("1234567890123456");
        p.setCardHolder("John Doe");
        p.setExpiryMonth("12");
        p.setExpiryYear("2025");
        p.setCvc("123");
        p.setSaveCard(false);
        p.setTotalAmount("100.00");
        p.setStatus(PaymentStatus.COMPLETED);
        p = paymentRepository.save(p);

        // Act & Assert
        mockMvc.perform(get("/payment/status/" + p.getId())
                .contentType(MediaType.APPLICATION_JSON))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$").value(PaymentStatus.COMPLETED.name()));

        // Verify DB record
        Payment fetched = paymentRepository.findById(p.getId()).get();
        assertThat(fetched.getStatus()).isEqualTo(PaymentStatus.COMPLETED);
    }
}*/