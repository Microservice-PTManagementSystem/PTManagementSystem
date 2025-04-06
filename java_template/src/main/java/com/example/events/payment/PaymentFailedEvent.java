// PaymentFailedEvent.java
package com.example.events.payment;

import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper = true)
public class PaymentFailedEvent extends PaymentEvent {
    private String reason;
    private String appointmentId;

    public PaymentFailedEvent(String paymentId, String reason, String appointmentId) {
        super(paymentId, "PAYMENT_FAILED");
        this.reason = reason;
        this.appointmentId = appointmentId;
    }
}