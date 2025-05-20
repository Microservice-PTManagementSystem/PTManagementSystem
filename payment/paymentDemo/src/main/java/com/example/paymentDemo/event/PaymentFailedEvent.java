package com.example.paymentDemo.event;

import java.io.Serializable;
import java.time.LocalDateTime;

import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.fasterxml.jackson.annotation.JsonProperty;

public class PaymentFailedEvent implements Serializable {
    @JsonProperty("slot_id")
    private String slotId;
    @JsonProperty("user_id")
    private String userId;
    //private String reason;
    @JsonProperty("timestamp")
    private String timestamp;
    private PaymentStatus status;
    private boolean isSuccess;

    public PaymentFailedEvent(Payment payment) {
        this.status = payment.getStatus();
        this.timestamp = timestamp;
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
    }
    public boolean isSuccess() {
        return isSuccess;
    }

    public void setSuccess(boolean isSuccess) {
        this.isSuccess = isSuccess;
    }

    public String getTimestamp() {
        return timestamp;
    }
    public void setTimestamp(String timestamp) {
        this.timestamp = timestamp;
    }
    public PaymentStatus getStatus() {
        return status;
    }

    public void setStatus(PaymentStatus status) {
        this.status = status;
    }
}