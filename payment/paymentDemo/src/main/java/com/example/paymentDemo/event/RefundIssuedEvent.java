package com.example.paymentDemo.event;

import com.example.paymentDemo.model.Payment;

import com.example.paymentDemo.model.PaymentStatus;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class RefundIssuedEvent {
    private Long paymentId;
    private String userId;
    private PaymentStatus status;
    private String paymentMethod;
 
    public RefundIssuedEvent(Payment payment) {
        this.paymentId = payment.getId();
        this.userId = payment.getUserId();
        this.status = payment.getStatus();
        this.paymentMethod = payment.getPaymentMethod();
    }
    public Long getPaymentId() {
        return paymentId;
    }

    public void setPaymentId(Long paymentId) {
        this.paymentId = paymentId;
    }
}
