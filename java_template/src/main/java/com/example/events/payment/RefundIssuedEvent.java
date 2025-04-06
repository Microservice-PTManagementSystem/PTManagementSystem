// RefundIssuedEvent.java
package com.example.events.payment;

import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper = true)
public class RefundIssuedEvent extends PaymentEvent {
    private String appointmentId;
    private Double amount;

    public RefundIssuedEvent(String paymentId, String appointmentId, Double amount) {
        super(paymentId, "REFUND_ISSUED");
        this.appointmentId = appointmentId;
        this.amount = amount;
    }
}