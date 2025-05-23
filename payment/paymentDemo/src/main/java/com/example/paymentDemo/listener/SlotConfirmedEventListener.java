package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.repository.PaymentRepository;

import com.example.paymentDemo.event.*;
import com.example.paymentDemo.dto.*;

@Component
public class SlotConfirmedEventListener {
    @Autowired
    private PaymentRepository paymentRepository;
    private final RabbitTemplate rabbitTemplate;
    //private final PaymentService paymentService;

    public SlotConfirmedEventListener(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
       // this.paymentService = paymentService;
        System.out.println("Slot Confirmed Initiated");
    }

    @RabbitListener(queues = "reservationQueue")
    public void handleSlotConfirmed(SlotReservedEvent event) {
        System.out.println("[PAYMENT] Message received: " + event);

        PaymentSucceededEvent paymentSucceeded = new PaymentSucceededEvent(
            event.getSlotId(),
            true

        );

        //paymentService.processPayment(event); 
        rabbitTemplate.convertAndSend("PaymentSucceededEvent", paymentSucceeded);
        System.out.println("PaymentSucceededEvent Published! "+ paymentSucceeded );
    }
    @RabbitListener(queues = "PaymentFailedEvent")
    public void handlePaymentFailed(PaymentFailedEvent event) {
        System.out.println("[PAYMENT] Message received: " + event);
        PaymentFailedEvent paymentFailed =new PaymentFailedEvent(
            false,
            event.getSlotId());
        //paymentService.processPayment(event);
        rabbitTemplate.convertAndSend("PaymentFailedEvent", paymentFailed);
        System.out.println("PaymentFailedEvent Published! "+ paymentFailed );
    }
    
    
}