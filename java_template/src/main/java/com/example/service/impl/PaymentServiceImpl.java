// PaymentServiceImpl.java
package com.example.service.impl;

import com.example.domain.Payment;
import com.example.domain.PaymentStatus;
import com.example.dto.PaymentRequest;
import com.example.dto.PaymentResponse;
import com.example.events.payment.*;
import com.example.exception.PaymentException;
import com.example.repository.PaymentRepository;
import com.example.service.PaymentService;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class PaymentServiceImpl implements PaymentService {
    private final PaymentRepository paymentRepository;
    private final RabbitTemplate rabbitTemplate;

    @Override
    @Transactional
    public PaymentResponse initiatePayment(PaymentRequest request) {
        Payment payment = new Payment();
        payment.setAppointmentId(request.getAppointmentId());
        payment.setUserId(request.getUserId());
        payment.setTrainerId(request.getTrainerId());
        payment.setAmount(BigDecimal.valueOf(request.getAmount()));
        payment.setStatus(PaymentStatus.PENDING);
        payment.setTransactionId(UUID.randomUUID().toString());

        payment = paymentRepository.save(payment);

        PaymentInitiatedEvent event = new PaymentInitiatedEvent(
            payment.getId().toString(),
            payment.getAppointmentId(),
            payment.getUserId(),
            payment.getAmount().doubleValue()
        );

        rabbitTemplate.convertAndSend("payment.exchange", "payment.initiated", event);

        return createPaymentResponse(payment);
    }

    @Override
    @Transactional
    public PaymentResponse processPayment(String paymentId) {
        Payment payment = paymentRepository.findById(Long.valueOf(paymentId))
            .orElseThrow(() -> new PaymentException("Payment not found"));

        try {
            // Simulate payment processing
            payment.setStatus(PaymentStatus.COMPLETED);
            payment = paymentRepository.save(payment);

            PaymentSucceededEvent event = new PaymentSucceededEvent(
                paymentId,
                payment.getTransactionId(),
                payment.getAppointmentId()
            );

            rabbitTemplate.convertAndSend("payment.exchange", "payment.succeeded", event);
        } catch (Exception e) {
            payment.setStatus(PaymentStatus.FAILED);
            payment.setErrorMessage(e.getMessage());
            payment = paymentRepository.save(payment);

            PaymentFailedEvent event = new PaymentFailedEvent(
                paymentId,
                e.getMessage(),
                payment.getAppointmentId()
            );

            rabbitTemplate.convertAndSend("payment.exchange", "payment.failed", event);
        }

        return createPaymentResponse(payment);
    }

    @Override
    public PaymentResponse getPaymentDetails(String paymentId) {
        Payment payment = paymentRepository.findById(Long.valueOf(paymentId))
            .orElseThrow(() -> new PaymentException("Payment not found"));
        return createPaymentResponse(payment);
    }

    @Override
    @Transactional
    public PaymentResponse refundPayment(String paymentId) {
        Payment payment = paymentRepository.findById(Long.valueOf(paymentId))
            .orElseThrow(() -> new PaymentException("Payment not found"));

        payment.setStatus(PaymentStatus.REFUNDED);
        payment = paymentRepository.save(payment);

        RefundIssuedEvent event = new RefundIssuedEvent(
            paymentId,
            payment.getAppointmentId(),
            payment.getAmount().doubleValue()
        );

        rabbitTemplate.convertAndSend("payment.exchange", "payment.refund", event);

        return createPaymentResponse(payment);
    }

    @Override
    public void handleSlotConfirmed(String appointmentId, String userId, Double amount) {
        PaymentRequest request = new PaymentRequest();
        request.setAppointmentId(appointmentId);
        request.setUserId(userId);
        request.setAmount(amount);
        initiatePayment(request);
    }

    @Override
    public void handleAppointmentCanceled(String appointmentId) {
        Payment payment = paymentRepository.findByAppointmentId(appointmentId)
            .orElseThrow(() -> new PaymentException("Payment not found for appointment"));
        refundPayment(payment.getId().toString());
    }

    private PaymentResponse createPaymentResponse(Payment payment) {
        PaymentResponse response = new PaymentResponse();
        response.setPaymentId(payment.getId().toString());
        response.setStatus(payment.getStatus());
        response.setAmount(payment.getAmount().doubleValue());
        response.setTransactionId(payment.getTransactionId());
        response.setCreatedAt(payment.getCreatedAt());
        response.setUpdatedAt(payment.getUpdatedAt());
        return response;
    }
}