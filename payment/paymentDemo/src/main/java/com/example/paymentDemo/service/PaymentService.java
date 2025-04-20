package com.example.paymentDemo.service;

import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

import com.example.paymentDemo.config.RabbitMQConfig;
import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.SlotReservedEvent;

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
            successEvent.setReservationId(event.getSlotId());
            //successEvent.setTrainerId(event.getTrainerId());
           // successEvent.setCustomerId(event.getCustomerId());

            rabbitTemplate.convertAndSend(RabbitMQConfig.EXCHANGE, "payment.success", successEvent);
        } else {
            PaymentFailedEvent failedEvent = new PaymentFailedEvent();
            failedEvent.setReservationId(event.getSlotId());
            //failedEvent.setCustomerId(event.getCustomerId());

            rabbitTemplate.convertAndSend(RabbitMQConfig.EXCHANGE, "payment.failed", failedEvent);
        }
    }

    private boolean fakePaymentGateway() {
        return Math.random() > 0.5;
    }
}


