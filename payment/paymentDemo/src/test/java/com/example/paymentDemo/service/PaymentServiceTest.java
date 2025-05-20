package com.example.paymentDemo.service;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.eq;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import org.mockito.MockitoAnnotations;
import org.springframework.amqp.rabbit.core.RabbitTemplate;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.event.PaymentInitiatedEvent;
import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.RefundIssuedEvent;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.repository.PaymentRepository;

public class PaymentServiceTest {
 
    @Mock
    private PaymentRepository paymentRepository;

    @Mock
    private RabbitTemplate rabbitTemplate;

    @InjectMocks
    private PaymentServiceImpl paymentService;

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

    @Test
    void shouldInitiatePaymentAndPublishEvent() {
        PaymentRequest request = new PaymentRequest("credit card","1234567890123456",
        "John Doe","12","2025","123",false,"100.00");

        Payment savedPayment = new Payment();
        savedPayment.setSlotId("1");
        savedPayment.setStatus(PaymentStatus.PENDING);

        when(paymentRepository.save(any(Payment.class))).thenReturn(savedPayment);

        Payment result = paymentService.initiatePayment(request);

        assertEquals(PaymentStatus.PENDING, result.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.initiated"), any(PaymentInitiatedEvent.class));
    }

    @Test
    void shouldConfirmPaymentAndPublishSucceededEvent() {
        Payment payment = new Payment();
        payment.setSlotId("1");
        payment.setStatus(PaymentStatus.PENDING);

        when(paymentRepository.findBySlotId("1")).thenReturn(Optional.of(payment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(payment);

        PaymentResult result = new PaymentResult();
        result.setSlotId("1");
        result.setSuccess(true);

        Payment confirmed = paymentService.confirmPayment(result);

        assertEquals(PaymentStatus.COMPLETED, confirmed.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.succeeded"), any(PaymentSucceededEvent.class));
    }

    @Test
    void shouldFailPaymentAndPublishFailedEvent() {
        Payment payment = new Payment();
        payment.setSlotId("1");
        payment.setStatus(PaymentStatus.PENDING);

        when(paymentRepository.findBySlotId("1")).thenReturn(Optional.of(payment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(payment);

        PaymentResult result = new PaymentResult();
        result.setSlotId("1");
        result.setSuccess(false);

        Payment failed = paymentService.confirmPayment(result);

        assertEquals(PaymentStatus.FAILED, failed.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.failed"), any(PaymentFailedEvent.class));
    }

    @Test
    void shouldRetryPaymentIfFailed() {
        Payment failedPayment = new Payment();
        failedPayment.setSlotId("1");
        failedPayment.setStatus(PaymentStatus.FAILED);
        failedPayment.setPaymentMethod("credit_card");
        failedPayment.setTotalAmount("50.0");

        when(paymentRepository.findBySlotId("1")).thenReturn(Optional.of(failedPayment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(failedPayment);

        paymentService.retryPayment("1");

        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.initiated"), any(PaymentInitiatedEvent.class));
    }

    @Test
    void shouldIssueRefundAndPublishRefundEvent() {
        Payment completedPayment = new Payment();
        completedPayment.setSlotId("1");
        completedPayment.setStatus(PaymentStatus.COMPLETED);
        completedPayment.setPaymentMethod("paypal");

        when(paymentRepository.findBySlotId("1")).thenReturn(Optional.of(completedPayment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(completedPayment);

        paymentService.issueRefund("1");

        assertEquals(PaymentStatus.REFUNDED, completedPayment.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.refunded"), any(RefundIssuedEvent.class));
    }

    @Test
    void shouldReturnCorrectPaymentStatus() {
        Payment payment = new Payment();
        payment.setSlotId("1");
        payment.setStatus(PaymentStatus.COMPLETED);

        when(paymentRepository.findBySlotId("1")).thenReturn(Optional.of(payment));

        PaymentStatus status = paymentService.getStatus("1");

        assertEquals(PaymentStatus.COMPLETED, status);
    }

    @Test
    void shouldThrowExceptionIfPaymentNotFoundWhenConfirming() {
    when(paymentRepository.findBySlotId("99")).thenReturn(Optional.empty());
    PaymentResult result = new PaymentResult();

    assertThrows(IllegalArgumentException.class, () -> paymentService.confirmPayment(result));
}

@Test
void shouldThrowExceptionIfPaymentNotFailedWhenRetrying() {
    Payment payment = new Payment();
    payment.setSlotId("1");
    payment.setStatus(PaymentStatus.COMPLETED);

    when(paymentRepository.findBySlotId("1")).thenReturn(Optional.of(payment));

    assertThrows(IllegalStateException.class, () -> paymentService.retryPayment("1"));
}

@Test
void shouldThrowIfPaymentStatusNotFound() {
    when(paymentRepository.findBySlotId("42")).thenReturn(Optional.empty());

    assertThrows(IllegalArgumentException.class, () -> paymentService.getStatus("42"));
}

@Test
void processPayment_Success() {
    // Arrange
    PaymentRequest request = new PaymentRequest("credit card","1234567890123456",
    "John Doe","12","2025","123",false,"100.00");
    request.setSaveCard(false);
    request.setTotalAmount("100.00");

    Payment savedPayment = new Payment();
    savedPayment.setSlotId("1");
    when(paymentRepository.save(any(Payment.class))).thenReturn(savedPayment);

    // Act
    PaymentResult result = paymentService.processPayment(request);

    // Assert
    assertTrue(result.success());
    assertEquals("1", result.getSlotId());
}

@Test
void processPayment_InvalidCardNumber() {
    // Arrange
    PaymentRequest request = new PaymentRequest("credit card","invalid",
    "John Doe","12","2025","123",false,"100.00");
    request.setSaveCard(false);
    request.setTotalAmount("100.00");

    Payment savedPayment = new Payment();
    savedPayment.setSlotId("1");
    when(paymentRepository.save(any(Payment.class))).thenReturn(savedPayment);

    // Act
    PaymentResult result = paymentService.processPayment(request);

    // Assert
    assertFalse(result.success());
}

}
