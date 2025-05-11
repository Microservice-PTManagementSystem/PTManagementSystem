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

    private Long paymentId;
    private String totalAmount;
    private String paymentMethod;
    private LocalDateTime timestamp;
 
    public PaymentSucceededEvent(String slotId) {
        this.slotId = slotId;
    }

    public String getslotId() {
        return slotId;
    }

    public void setslotId(String slotId) {
        this.slotId = slotId;
    }
    public Long getPaymentId() {
        return paymentId;
    }
    public void setPaymentId(Long paymentId) {
        this.paymentId = paymentId;
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
        this.paymentId = payment.getId();
        this.totalAmount = payment.getTotalAmount();
        this.paymentMethod = payment.getPaymentMethod();
        this.timestamp = LocalDateTime.now();
    }
}
