package com.example.paymentDemo.dto;

import com.example.paymentDemo.model.PaymentStatus;
import com.fasterxml.jackson.annotation.JsonProperty;

public class InitiateResponse {
    private PaymentStatus status;
    private String message;
    @JsonProperty("slot_id")
    private String slotId;
    @JsonProperty("user_id")
    private String userId;

    public InitiateResponse(PaymentStatus status, String message, String slotId, String userId) {
        this.status = status;
        this.message = message;
        this.slotId = slotId;
        this.userId = userId;
    }

    // Getters & Setters

    public PaymentStatus getStatus() {
        return status;
    }

    public void setStatus(PaymentStatus status) {
        this.status = status;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
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
}
