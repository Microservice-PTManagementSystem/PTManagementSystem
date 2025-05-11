package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Component;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.example.paymentDemo.event.PaymentInitiatedEvent;
import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.event.RefundIssuedEvent;

import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.SlotReservedEvent;

@Component
public class SlotConfirmedEventListener {

    private final RabbitTemplate rabbitTemplate;
    private static final Logger logger = LoggerFactory.getLogger(SlotConfirmedEventListener.class);
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
            event.getSlotId()
        );

        //paymentService.processPayment(event); 
        rabbitTemplate.convertAndSend("PaymentSucceededEvent", paymentSucceeded);
        System.out.println("PaymentSucceededEvent Published! "+ paymentSucceeded );
    }
    /* 

    @RabbitListener(queues = "paymentInitiatedEvent")
    public void handlePaymentInitiated(PaymentInitiatedEvent event) {
        logger.info("Payment Initiated Event Received - Payment ID: {}, Amount: {}, Method: {}", 
            event.getPaymentId(), event.getTotalAmount(), event.getPaymentMethod());
    }
    
    @RabbitListener(queues = "paymentSucceededEvent")
    public void handlePaymentSucceeded(PaymentSucceededEvent event) {
        logger.info("Payment Succeeded Event Received - Payment ID: {}, Amount: {}, Method: {}", 
            event.getPaymentId(), event.getTotalAmount(), event.getPaymentMethod());
    }

    @RabbitListener(queues = "paymentFailedEvent")
    public void handlePaymentFailed(PaymentFailedEvent event) {
        logger.info("Payment Failed Event Received - Payment ID: {}, Status: {}", 
            event.getPaymentId(), event.getStatus());
    }

    @RabbitListener(queues = "paymentRefundedEvent")
    public void handlePaymentRefunded(RefundIssuedEvent event) {
        logger.info("Payment Refunded Event Received - Payment ID: {}", 
            event.getPaymentId());
    }*/
}
