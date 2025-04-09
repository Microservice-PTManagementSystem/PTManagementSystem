package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

import com.example.paymentDemo.event.SlotReservedEvent;
import com.example.paymentDemo.service.PaymentService;

@Component
public class PaymentEventListener {

    private final PaymentService paymentService;

    public PaymentEventListener(PaymentService paymentService) {
        this.paymentService = paymentService;
    }

    @RabbitListener(queues = "slot.reserved.queue")
    public void handleSlotReservedEvent(SlotReservedEvent event) {
        paymentService.processPayment(event);
    }
}
