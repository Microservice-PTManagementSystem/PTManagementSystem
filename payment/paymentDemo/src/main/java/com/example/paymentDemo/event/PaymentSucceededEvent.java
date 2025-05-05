package com.example.paymentDemo.event;

import java.time.LocalDateTime;

import com.example.paymentDemo.model.Payment;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class PaymentSucceededEvent {
    private String slotId;
    private String trainerId;
    private String customerId;

    private Long paymentId;
    private Double amount;
    private String method;
    private LocalDateTime timestamp;
 
    public PaymentSucceededEvent(String slotId, String trainerId, String customerId) {
        this.slotId = slotId;
        this.trainerId = trainerId;
        this.customerId = customerId;
    }

    public PaymentSucceededEvent(Payment payment) {
        this.paymentId = payment.getId();
        this.amount = payment.getAmount();
        this.method = payment.getMethod();
        this.timestamp = LocalDateTime.now();
    }
}
