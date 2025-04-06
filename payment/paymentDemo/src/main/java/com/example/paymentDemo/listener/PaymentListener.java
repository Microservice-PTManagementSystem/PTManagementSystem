package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

@Component
public class PaymentListener {

    @RabbitListener(queues = "payment_queue")
    public void receiveMessage(String message) {
        System.out.println("Received payment message: " + message);
    }
}
