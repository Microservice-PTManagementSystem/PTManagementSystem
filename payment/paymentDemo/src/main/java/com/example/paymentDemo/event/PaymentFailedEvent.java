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
    private boolean isFailed;

    public PaymentFailedEvent(Payment payment) {
        this.status = payment.getStatus();
        this.timestamp = timestamp;
    } 
    public PaymentFailedEvent(boolean isFailed, String slotId) {
       this.isFailed = isFailed;
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
    public boolean isFailed() {
        return isFailed;
    }

    public void setFailed(boolean isFailed) {
        this.isFailed = isFailed;
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