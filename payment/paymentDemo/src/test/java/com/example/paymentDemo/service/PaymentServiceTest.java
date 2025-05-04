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
        PaymentRequest request = new PaymentRequest("2451",100.0,"credit card","billing1",105486972L);

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

        PaymentResult result = new PaymentResult(1L, true, "TXN-002",null);

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

        PaymentResult result = new PaymentResult(1L, false,"TXN-001", "Declined");

        Payment failed = paymentService.confirmPayment(result);

        assertEquals(PaymentStatus.FAILED, failed.getStatus());
        verify(rabbitTemplate).convertAndSend(eq("payment.exchange"), eq("payment.failed"), any(PaymentFailedEvent.class));
    }

    @Test
    void shouldRetryPaymentIfFailed() {
        Payment failedPayment = new Payment();
        failedPayment.setId(1L);
        failedPayment.setStatus(PaymentStatus.FAILED);
        failedPayment.setUserId("user9");
        failedPayment.setMethod("credit_card");
        failedPayment.setAmount(50.0);

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
        completedPayment.setUserId("user11");
        completedPayment.setMethod("paypal");

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
    PaymentResult result = new PaymentResult(99L, true, "TXN-404", null);

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



}
