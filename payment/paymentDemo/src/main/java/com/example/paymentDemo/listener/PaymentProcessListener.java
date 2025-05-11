package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Component;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.example.paymentDemo.event.PaymentProcessEvent;
import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.repository.PaymentRepository;

@Component
public class PaymentProcessListener {
    private static final Logger logger = LoggerFactory.getLogger(PaymentProcessListener.class);
    private final RabbitTemplate rabbitTemplate;
    private final PaymentRepository paymentRepository;

    public PaymentProcessListener(RabbitTemplate rabbitTemplate, PaymentRepository paymentRepository) {
        this.rabbitTemplate = rabbitTemplate;
        this.paymentRepository = paymentRepository;
    }

    @RabbitListener(queues = "payment.process.queue")
    public void handlePaymentProcess(PaymentProcessEvent event) {
        logger.info("Received payment process event for appointment: {}", event.getAppointmentId());
        
        try {
            // Create and save payment
            Payment payment = new Payment();
            payment.setAppointmentId(event.getAppointmentId());
            payment.setUserId(event.getUserId());
            payment.setPaymentMethod(event.getPaymentMethod());
            payment.setCardNumber(event.getCardNumber());
            payment.setCardHolder(event.getCardHolder());
            payment.setExpiryMonth(event.getExpiryMonth());
            payment.setExpiryYear(event.getExpiryYear());
            payment.setCvc(event.getCvc());
            payment.setSaveCard(event.isSaveCard());
            payment.setTotalAmount(event.getTotalAmount());
            payment.setSlotId(event.getSlotId());
            payment.setStatus(PaymentStatus.PENDING);

            Payment savedPayment = paymentRepository.save(payment);

            // Simulate bank communication and payment processing
            boolean paymentSuccess = processPaymentWithBank(event);

            if (paymentSuccess) {
                // Update payment status and publish success event
                savedPayment.setStatus(PaymentStatus.COMPLETED);
                paymentRepository.save(savedPayment);
                
                PaymentSucceededEvent successEvent = new PaymentSucceededEvent(savedPayment);
                rabbitTemplate.convertAndSend("payment.exchange", "payment.succeeded", successEvent);
                
                logger.info("Payment processed successfully for appointment: {}", event.getAppointmentId());
            } else {
                // Update payment status and publish failure event
                savedPayment.setStatus(PaymentStatus.FAILED);
                paymentRepository.save(savedPayment);
                
                PaymentFailedEvent failedEvent = new PaymentFailedEvent(savedPayment);
                rabbitTemplate.convertAndSend("payment.exchange", "payment.failed", failedEvent);
                
                logger.info("Payment processing failed for appointment: {}", event.getAppointmentId());
            }
        } catch (Exception e) {
            logger.error("Error processing payment for appointment: " + event.getAppointmentId(), e);
            // Publish failure event in case of any error
            PaymentFailedEvent failedEvent = new PaymentFailedEvent();
            failedEvent.setPaymentId(null);
            failedEvent.setStatus(PaymentStatus.FAILED);
            rabbitTemplate.convertAndSend("payment.exchange", "payment.failed", failedEvent);
        }
    }

    private boolean processPaymentWithBank(PaymentProcessEvent event) {
        // Simulate bank communication
        // In a real application, this would make an actual API call to a bank
        try {
            // Simulate network delay
            Thread.sleep(1000);
            
            // Simple validation for demo purposes
            return event.getCardNumber() != null && 
                   event.getCardNumber().matches("\\d{16}") &&
                   event.getTotalAmount() != null &&
                   Double.parseDouble(event.getTotalAmount()) > 0;
        } catch (Exception e) {
            logger.error("Error communicating with bank", e);
            return false;
        }
    }
} 