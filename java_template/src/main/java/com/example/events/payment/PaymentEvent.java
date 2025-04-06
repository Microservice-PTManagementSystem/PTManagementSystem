// PaymentEvent.java
package com.example.events.payment;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public abstract class PaymentEvent {
    private String paymentId;
    private LocalDateTime timestamp;
    private String eventType;

    public PaymentEvent(String paymentId, String eventType) {
        this.paymentId = paymentId;
        this.timestamp = LocalDateTime.now();
        this.eventType = eventType;
    }
}