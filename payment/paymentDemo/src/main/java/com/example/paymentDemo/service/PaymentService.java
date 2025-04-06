package com.example.paymentDemo.service;

import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

@Service
public class PaymentService {

    private final RabbitTemplate rabbitTemplate;

    public PaymentService(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    public void sendPaymentMessage(String message) {
        rabbitTemplate.convertAndSend("payment_queue", message);
        System.out.println("Payment message sent: " + message);
    }
}
