package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Component;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.event.PaymentSucceededEvent;
import com.example.paymentDemo.event.SlotReservedEvent;
import com.example.paymentDemo.service.IyzicoPaymentService;

@Component
public class SlotConfirmedEventListener {

    private final RabbitTemplate rabbitTemplate;
    private final IyzicoPaymentService iyzicoPaymentService; // Ödeme servisini entegre ettik
    private static final Logger logger = LoggerFactory.getLogger(SlotConfirmedEventListener.class);

    public SlotConfirmedEventListener(RabbitTemplate rabbitTemplate, IyzicoPaymentService iyzicoPaymentService) {
        this.rabbitTemplate = rabbitTemplate;
        this.iyzicoPaymentService = iyzicoPaymentService;
        logger.info("SlotConfirmedEventListener initialized");
    }

    @RabbitListener(queues = "reservationQueue")
    public void handleSlotConfirmed(SlotReservedEvent event) {
        logger.info("[PAYMENT] Slot reserved event received: {}", event);

        try {
            // Iyzico ile ödeme işlemini gerçekleştirme
            boolean paymentResult = iyzicoPaymentService.processPayment(event);

            if (paymentResult) {
                PaymentSucceededEvent paymentSucceeded = new PaymentSucceededEvent(
                    event.getSlotId(),
                    event.getUserId()
                );
                rabbitTemplate.convertAndSend("paymentSucceededQueue", paymentSucceeded);
                logger.info("PaymentSucceededEvent published: {}", paymentSucceeded);
            } else {
                PaymentFailedEvent paymentFailed = new PaymentFailedEvent(
                    event.getSlotId(),
                    event.getUserId()
                );
                rabbitTemplate.convertAndSend("paymentFailedQueue", paymentFailed);
                logger.warn("PaymentFailedEvent published: {}", paymentFailed);
            }
        } catch (Exception e) {
            logger.error("Payment processing failed with exception: ", e);
            PaymentFailedEvent paymentFailed = new PaymentFailedEvent(
                event.getSlotId(),
                event.getUserId()
            );
            rabbitTemplate.convertAndSend("paymentFailedQueue", paymentFailed);
        }
    }
}
