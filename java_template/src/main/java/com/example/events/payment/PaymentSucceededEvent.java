// PaymentSucceededEvent.java
package com.example.events.payment;

import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper = true)
public class PaymentSucceededEvent extends PaymentEvent {
    private String transactionId;
    private String appointmentId;

    public PaymentSucceededEvent(String paymentId, String transactionId, String appointmentId) {
        super(paymentId, "PAYMENT_SUCCEEDED");
        this.transactionId = transactionId;
        this.appointmentId = appointmentId;
    }
}