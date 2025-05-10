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

    public String getslotId() {
        return slotId;
    }

    public void setslotId(String slotId) {
        this.slotId = slotId;
    }

    public String getTrainerId() {
        return trainerId;
    }

    public void setTrainerId(String trainerId) {
        this.trainerId = trainerId;
    }

    public String getCustomerId() {
        return customerId;
    }

    public void setCustomerId(String customerId) {
        this.customerId = customerId;
    }
    public Long getPaymentId() {
        return paymentId;
    }
    public void setPaymentId(Long paymentId) {
        this.paymentId = paymentId;
    }
    public Double getAmount() {
        return amount;
    }
    public void setAmount(Double amount) {
        this.amount = amount;
    }
    public String getMethod() {
        return method;
    }
    public void setMethod(String method) {
        this.method = method;
    }
    public LocalDateTime getTimestamp() {
        return timestamp;
    }
    public void setTimestamp(LocalDateTime timestamp) {
        this.timestamp = timestamp;
    }
    
    public PaymentSucceededEvent(Payment payment) {
        this.paymentId = payment.getId();
        this.amount = payment.getAmount();
        this.method = payment.getMethod();
        this.timestamp = LocalDateTime.now();
    }
}
