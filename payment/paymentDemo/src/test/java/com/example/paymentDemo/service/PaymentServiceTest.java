package com.example.paymentDemo.service;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.event.*;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.repository.PaymentRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.springframework.amqp.rabbit.core.RabbitTemplate;

import java.util.*;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

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
        savedPayment.setId(1L);
        savedPayment.setStatus(PaymentStatus.PENDING);

        when(paymentRepository.save(any(Payment.class))).thenReturn(savedPayment);

        Payment result = paymentService.initiatePayment(request);

        assertEquals(PaymentStatus.PENDING, result.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.initiated"), any(PaymentInitiatedEvent.class));
    }

    @Test
    void shouldConfirmPaymentAndPublishSucceededEvent() {
        Payment payment = new Payment();
        payment.setId(1L);
        payment.setStatus(PaymentStatus.PENDING);

        when(paymentRepository.findById(1L)).thenReturn(Optional.of(payment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(payment);

        PaymentResult result = new PaymentResult();
        result.setPaymentId(1L);
        result.setSuccess(true);
        result.setMessage("Payment processed successfully");

        Payment confirmed = paymentService.confirmPayment(result);

        assertEquals(PaymentStatus.COMPLETED, confirmed.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.succeeded"), any(PaymentSucceededEvent.class));
    }

    @Test
    void shouldFailPaymentAndPublishFailedEvent() {
        Payment payment = new Payment();
        payment.setId(1L);
        payment.setStatus(PaymentStatus.PENDING);

        when(paymentRepository.findById(1L)).thenReturn(Optional.of(payment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(payment);

        PaymentResult result = new PaymentResult();
        result.setPaymentId(1L);
        result.setSuccess(false);
        result.setMessage("Payment failed");

        Payment failed = paymentService.confirmPayment(result);

        assertEquals(PaymentStatus.FAILED, failed.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.failed"), any(PaymentFailedEvent.class));
    }

    @Test
    void shouldRetryPaymentIfFailed() {
        Payment failedPayment = new Payment();
        failedPayment.setId(1L);
        failedPayment.setStatus(PaymentStatus.FAILED);
        failedPayment.setPaymentMethod("credit_card");
        failedPayment.setTotalAmount("50.0");

        when(paymentRepository.findById(1L)).thenReturn(Optional.of(failedPayment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(failedPayment);

        paymentService.retryPayment(1L);

        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.initiated"), any(PaymentInitiatedEvent.class));
    }

    @Test
    void shouldIssueRefundAndPublishRefundEvent() {
        Payment completedPayment = new Payment();
        completedPayment.setId(1L);
        completedPayment.setStatus(PaymentStatus.COMPLETED);
        completedPayment.setPaymentMethod("paypal");

        when(paymentRepository.findById(1L)).thenReturn(Optional.of(completedPayment));
        when(paymentRepository.save(any(Payment.class))).thenReturn(completedPayment);

        paymentService.issueRefund(1L);

        assertEquals(PaymentStatus.REFUNDED, completedPayment.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.refunded"), any(RefundIssuedEvent.class));
    }

    @Test
    void shouldReturnCorrectPaymentStatus() {
        Payment payment = new Payment();
        payment.setId(1L);
        payment.setStatus(PaymentStatus.COMPLETED);

        when(paymentRepository.findById(1L)).thenReturn(Optional.of(payment));

        PaymentStatus status = paymentService.getStatus(1L);

        assertEquals(PaymentStatus.COMPLETED, status);
    }

    @Test
    void shouldThrowExceptionIfPaymentNotFoundWhenConfirming() {
    when(paymentRepository.findById(99L)).thenReturn(Optional.empty());
    PaymentResult result = new PaymentResult();

    assertThrows(IllegalArgumentException.class, () -> paymentService.confirmPayment(result));
}

@Test
void shouldThrowExceptionIfPaymentNotFailedWhenRetrying() {
    Payment payment = new Payment();
    payment.setId(1L);
    payment.setStatus(PaymentStatus.COMPLETED);

    when(paymentRepository.findById(1L)).thenReturn(Optional.of(payment));

    assertThrows(IllegalStateException.class, () -> paymentService.retryPayment(1L));
}

@Test
void shouldThrowIfPaymentStatusNotFound() {
    when(paymentRepository.findById(42L)).thenReturn(Optional.empty());

    assertThrows(IllegalArgumentException.class, () -> paymentService.getStatus(42L));
}

@Test
void processPayment_Success() {
    // Arrange
    PaymentRequest request = new PaymentRequest("credit card","1234567890123456",
    "John Doe","12","2025","123",false,"100.00");
    request.setSaveCard(false);
    request.setTotalAmount("100.00");

    Payment savedPayment = new Payment();
    savedPayment.setId(1L);
    when(paymentRepository.save(any(Payment.class))).thenReturn(savedPayment);

    // Act
    PaymentResult result = paymentService.processPayment(request);

    // Assert
    assertTrue(result.success());
    assertNotNull(result.getMessage());
    assertEquals("Payment processed successfully", result.getMessage());
    assertEquals(1L, result.getPaymentId());
}

@Test
void processPayment_InvalidCardNumber() {
    // Arrange
    PaymentRequest request = new PaymentRequest("credit card","invalid",
    "John Doe","12","2025","123",false,"100.00");
    request.setSaveCard(false);
    request.setTotalAmount("100.00");

    Payment savedPayment = new Payment();
    savedPayment.setId(1L);
    when(paymentRepository.save(any(Payment.class))).thenReturn(savedPayment);

    // Act
    PaymentResult result = paymentService.processPayment(request);

    // Assert
    assertFalse(result.success());
    assertNotNull(result.getMessage());
    assertTrue(result.getMessage().contains("Payment failed"));
}

}
