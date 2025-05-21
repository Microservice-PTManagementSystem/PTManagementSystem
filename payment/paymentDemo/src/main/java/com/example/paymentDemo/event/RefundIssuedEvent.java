package com.example.paymentDemo.event;

import com.example.paymentDemo.model.Payment;

import com.example.paymentDemo.model.PaymentStatus;
import com.fasterxml.jackson.annotation.JsonProperty;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class RefundIssuedEvent {
    @JsonProperty("user_id")
    private String userId;
    private PaymentStatus status;
    @JsonProperty("paymentMethod")
    private String paymentMethod;
 
    public RefundIssuedEvent(Payment payment) {
        this.userId = payment.getUserId();
        this.status = payment.getStatus();
        this.paymentMethod = payment.getPaymentMethod();
    }
}