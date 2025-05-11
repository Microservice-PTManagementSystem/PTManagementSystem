package com.example.paymentDemo.event;

import java.io.Serializable;
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
    private String userId;

    private String totalAmount;
    private String paymentMethod;
    private LocalDateTime timestamp;
 
    public PaymentSucceededEvent(String slotId, String userId) {
        this.slotId = slotId;
        this.userId = userId;
    }

    public String getslotId() {
        return slotId;
    }

    public void setslotId(String slotId) {
        this.slotId = slotId;
    }
    public String getUserId() {
        return userId;
    }
    public void setUserId(String userId) {
        this.userId = userId;
    }
    public String getTotalAmount() {
        return totalAmount;
    }
    public void setTotalAmount(String totalAmount) {
        this.totalAmount = totalAmount;
    }
    public String getPaymentMethod() {
        return paymentMethod;
    }
    public void setPaymentMethod(String paymentMethod) {
        this.paymentMethod = paymentMethod;
    }
    public LocalDateTime getTimestamp() {
        return timestamp;
    }
    public void setTimestamp(LocalDateTime timestamp) {
        this.timestamp = timestamp;
    }
    
    public PaymentSucceededEvent(Payment payment) {
        this.userId = payment.getUserId();
        this.totalAmount = payment.getTotalAmount();
        this.paymentMethod = payment.getPaymentMethod();
        this.timestamp = LocalDateTime.now();
    }
}
