package com.example.paymentDemo.service;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.event.*;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.repository.PaymentRepository;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.*;
 
@Service
public class PaymentServiceImpl implements PaymentService {

    private final PaymentRepository paymentRepository;
    private final RabbitTemplate rabbitTemplate;

    private final String PAYMENT_EXCHANGE = "payment.exchange";

    public PaymentServiceImpl(PaymentRepository paymentRepository,
    RabbitTemplate rabbitTemplate) {
        this.paymentRepository = paymentRepository;
        this.rabbitTemplate = rabbitTemplate;
}

    @Override
    public Payment initiatePayment(PaymentRequest request) {
        Payment payment = new Payment();
        payment.setPaymentMethod(request.getPaymentMethod());
        payment.setCardNumber(request.getCardNumber());
        payment.setCardHolder(request.getCardHolder());
        payment.setExpiryMonth(request.getExpiryMonth());
        payment.setExpiryYear(request.getExpiryYear());
        payment.setCvc(request.getCvc());
        payment.setSaveCard(request.isSaveCard());
        payment.setTotalAmount(request.getTotalAmount());
        payment.setStatus(PaymentStatus.PENDING);

        Payment saved = paymentRepository.save(payment);
        rabbitTemplate.convertAndSend(PAYMENT_EXCHANGE, "payment.initiated", new PaymentInitiatedEvent(saved));
        return saved;
    }

    @Override
    public Payment confirmPayment(PaymentResult result) {
        Payment payment = paymentRepository.findById(result.getPaymentId())
                .orElseThrow(() -> new IllegalArgumentException("Payment not found"));

        if (result.success()) {
            payment.setStatus(PaymentStatus.COMPLETED);
            rabbitTemplate.convertAndSend(PAYMENT_EXCHANGE, "payment.succeeded", new PaymentSucceededEvent(payment));
        } else {
            payment.setStatus(PaymentStatus.FAILED);
            rabbitTemplate.convertAndSend(PAYMENT_EXCHANGE, "payment.failed", new PaymentFailedEvent(payment));
        }

        return paymentRepository.save(payment);
    }

    @Override
    public Payment retryPayment(Long paymentId) {
        Payment payment = paymentRepository.findById(paymentId)
                .orElseThrow(() -> new IllegalArgumentException("Payment not found"));

        if (!PaymentStatus.FAILED.equals(payment.getStatus())) {
            throw new IllegalStateException("Only failed payments can be retried");
        }

        payment.setStatus(PaymentStatus.PENDING);
        paymentRepository.save(payment);

        rabbitTemplate.convertAndSend(PAYMENT_EXCHANGE, "payment.initiated", new PaymentInitiatedEvent(payment));
        return payment;
    }

    @Override
    public Payment issueRefund(Long paymentId) {
        Payment payment = paymentRepository.findById(paymentId)
                .orElseThrow(() -> new IllegalArgumentException("Payment not found"));

        payment.setStatus(PaymentStatus.REFUNDED);
        Payment refunded = paymentRepository.save(payment);

        rabbitTemplate.convertAndSend(PAYMENT_EXCHANGE, "payment.refunded", new RefundIssuedEvent(refunded));
        return refunded;
    }

    @Override
    public PaymentStatus getStatus(Long paymentId) {
        return paymentRepository.findById(paymentId)
                .map(Payment::getStatus)
                .orElseThrow(() -> new IllegalArgumentException("Payment not found"));
    }

    @Override
    public PaymentResult processPayment(PaymentRequest request) {
        Payment payment = new Payment();
        payment.setPaymentMethod(request.getPaymentMethod());
        payment.setCardNumber(request.getCardNumber());
        payment.setCardHolder(request.getCardHolder());
        payment.setExpiryMonth(request.getExpiryMonth());
        payment.setExpiryYear(request.getExpiryYear());
        payment.setCvc(request.getCvc());
        payment.setSaveCard(request.isSaveCard());
        payment.setTotalAmount(request.getTotalAmount());
        payment.setStatus(PaymentStatus.PENDING);

        Payment savedPayment = paymentRepository.save(payment);

        PaymentResult result = new PaymentResult();
        // Check if card number is valid (simple validation for demo)
        boolean isValidCard = request.getCardNumber() != null && 
                            request.getCardNumber().matches("\\d{16}");
        
        result.setSuccess(isValidCard);
        result.setMessage(isValidCard ? "Payment processed successfully" : "Payment failed");
        result.setCardNumber(request.getCardNumber());
        result.setCardHolder(request.getCardHolder());
        result.setExpiryMonth(request.getExpiryMonth());
        result.setExpiryYear(request.getExpiryYear());
        result.setCvc(request.getCvc());
        result.setSaveCard(request.isSaveCard());
        result.setTotalAmount(request.getTotalAmount());
        result.setPaymentMethod(request.getPaymentMethod());
        result.setPaymentId(savedPayment.getId());

        return result;
    }

}
