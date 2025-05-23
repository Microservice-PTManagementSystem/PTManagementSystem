/*package com.example.paymentDemo.listener;


import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.event.PaymentInfoRequested;

@Component
public class PaymentInfoRequestedListener {

   @RabbitListener(queues = "PaymentInfoRequested")
    public void handleGetSavedCard(PaymentInfoRequested event) {
    System.out.println("GELEN EVENT: " + event);
    System.out.println("useSavedCard: " + (event != null ? event.isUseSavedCard() : "null"));
    if (event != null && event.isUseSavedCard()) {
        System.out.println("Saved card is used.");
    } else {
        System.out.println("Saved card is NOT used.");
    }
}

}*/
