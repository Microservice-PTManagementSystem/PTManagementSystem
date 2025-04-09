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
/*@Component
public class PaymentListener {

    @RabbitListener(queues = "payment.queue")
    public void handlePayment(PaymentRequest request) {
        // Ödeme işleme mantığı buraya yazılır.
        System.out.println("Payment received: " + request);
    }
}
 */