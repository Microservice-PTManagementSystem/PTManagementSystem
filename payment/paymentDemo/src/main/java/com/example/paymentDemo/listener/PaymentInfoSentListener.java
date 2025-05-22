package com.example.paymentDemo.listener;

import com.example.paymentDemo.event.PaymentInfoSentEvent;
import com.example.paymentDemo.dto.PaymentRequest;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

@Component
public class PaymentInfoSentListener {

   /* @RabbitListener(queues = "PaymentInfoSentEvent")
    public PaymentRequest handlePaymentInfoSent(PaymentInfoSentEvent event) {
        System.out.println("PaymentInfo alındı: " + event);

        var dto = event.getPaymentInfo();
        PaymentRequest request = new PaymentRequest();
        request.setCardHolder(dto.getCardHolder());
        request.setCardNumber(dto.getCardNumber());
        request.setExpiryMonth(dto.getExpiryMonth());
        request.setExpiryYear(dto.getExpiryYear());
        request.setCvc(dto.getCvc());

        // Sabit/ön tanımlı değerler atanabilir veya gerekli ek bilgiler eklenebilir
        request.setUserId(event.getUserId());

        return request;
    }*/
}