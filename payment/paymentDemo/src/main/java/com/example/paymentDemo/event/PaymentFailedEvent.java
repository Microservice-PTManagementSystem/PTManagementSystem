package com.example.paymentDemo.event;

import java.io.Serializable;
import java.time.LocalDateTime;

import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;

public class PaymentFailedEvent implements Serializable {
    private String slotId;
    private String userId;
    //private String reason;

    private LocalDateTime timestamp;
    private PaymentStatus status;
    private boolean isSuccess;

    public PaymentFailedEvent(Payment payment) {
        this.status = payment.getStatus();
        this.timestamp = LocalDateTime.now();
    } 
    public PaymentFailedEvent(String userId, String slotId) {
       this.userId = userId;
       this.slotId = slotId;
       //this.reason = reason;
        
    }

    public String getSlotId() {
        return slotId;
    }
    public void setSlotId(String slotId) {
        this.slotId = slotId;
    }
    public String getUserId() {
        return userId;
    }
    public void setUserId(String userId) {
        this.userId = userId;
    }/*
    public String getReason() {
        return reason;
    }
    public void setReason(String reason) {
        this.reason = reason;
    }*/
    public boolean isSuccess() {
        return isSuccess;
    }

    public void setSuccess(boolean isSuccess) {
        this.isSuccess = isSuccess;
    }

    public LocalDateTime getTimestamp() {
        return timestamp;
    }
    public void setTimestamp(LocalDateTime timestamp) {
        this.timestamp = timestamp;
    }
    public PaymentStatus getStatus() {
        return status;
    }

    public void setStatus(PaymentStatus status) {
        this.status = status;
    }
}
