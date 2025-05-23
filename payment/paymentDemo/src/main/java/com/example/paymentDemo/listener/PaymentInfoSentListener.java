package com.example.paymentDemo.listener;

import java.util.Map;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;

import com.example.paymentDemo.event.PaymentInfoSentEvent;
import com.example.paymentDemo.dto.PaymentRequestNew;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.example.paymentDemo.dto.PaymentInfoDto;
import com.example.paymentDemo.util.PaymentInfoHolder;

@Component
public class PaymentInfoSentListener {

    /*@RabbitListener(queues = "PaymentInfoSentEvent")
    public PaymentRequestNew handlePaymentInfoSent(PaymentInfoSentEvent event) {
        System.out.println("PaymentInfo alındı: " + event.getPaymentInfo());

        var dto = event.getPaymentInfo();
        PaymentRequestNew request = new PaymentRequestNew();
        request.setCardHolder(dto.getCardHolder());
        request.setCardNumber(dto.getCardNumber());
        request.setExpiryMonth(dto.getExpiryMonth());
        request.setExpiryYear(dto.getExpiryYear());
        request.setCvc(dto.getCvc());

        request.setUserId(event.getUserId());

        return request;
    }*/

   @Autowired
    private PaymentInfoHolder paymentInfoHolder;

    @RabbitListener(queues = "PaymentInfoSentEvent")
    public void handlePaymentInfo(PaymentInfoSentEvent event) {
        System.out.println("[PaymentInfoListener] Cevap geldi: " + event.getPaymentInfo());
        paymentInfoHolder.complete(event.getUserId(), event.getPaymentInfo());
    }


}