package com.example.paymentDemo.event;

import com.example.paymentDemo.model.Payment;

public class PaymentInitiatedEvent {
    private String appointmentId;
    private String userId;
    private Double amount;
    private Payment saved;
    // getters and setters

    public PaymentInitiatedEvent(Payment saved) {
        this.saved = saved;
        this.userId = String.valueOf(saved.getUserId());
        this.amount = saved.getAmount();
        this.appointmentId = String.valueOf(saved.getAppointmentId());
    }
    
    public String getAppointmentId() {
        return appointmentId;
    }

    public void setAppointmentId(String appointmentId) {
        this.appointmentId = appointmentId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public Double getAmount() {
        return amount;
    }

    public void setAmount(Double amount) {
        this.amount = amount;
    }
}
