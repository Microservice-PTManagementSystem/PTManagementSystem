// PaymentInitiatedEvent.java
package com.example.events.payment;

import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper = true)
public class PaymentInitiatedEvent extends PaymentEvent {
    private String appointmentId;
    private String userId;
    private Double amount;

    public PaymentInitiatedEvent(String paymentId, String appointmentId, String userId, Double amount) {
        super(paymentId, "PAYMENT_INITIATED");
        this.appointmentId = appointmentId;
        this.userId = userId;
        this.amount = amount;
    }
}