package com.example.paymentDemo.service;

import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;
/* @Service
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
*/

import com.example.paymentDemo.config.RabbitMQConfig;
import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.SlotReservedEvent;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

@Service
public class PaymentService {

    private final RabbitTemplate rabbitTemplate;

    public PaymentService(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    public void processPayment(SlotReservedEvent event) {
        boolean paymentSuccess = fakePaymentGateway();

        if (paymentSuccess) {
            PaymentSucceededEvent successEvent = new PaymentSucceededEvent();
            successEvent.setReservationId(event.getId());
            successEvent.setTrainerId(event.getTrainerId());
            successEvent.setCustomerId(event.getCustomerId());

            rabbitTemplate.convertAndSend(RabbitMQConfig.EXCHANGE, "payment.success", successEvent);
        } else {
            PaymentFailedEvent failedEvent = new PaymentFailedEvent();
            failedEvent.setReservationId(event.getId());
            failedEvent.setCustomerId(event.getCustomerId());

            rabbitTemplate.convertAndSend(RabbitMQConfig.EXCHANGE, "payment.failed", failedEvent);
        }
    }

    private boolean fakePaymentGateway() {
        return Math.random() > 0.5;
    }
}


