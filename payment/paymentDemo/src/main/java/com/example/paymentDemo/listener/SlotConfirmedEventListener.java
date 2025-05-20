package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Component;

import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.SlotReservedEvent;

@Component
public class SlotConfirmedEventListener {

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
            event.getUserId()
        );

        //paymentService.processPayment(event); 
        rabbitTemplate.convertAndSend("PaymentSucceededEvent", paymentSucceeded);
        System.out.println("PaymentSucceededEvent Published! "+ paymentSucceeded );
    }
}
