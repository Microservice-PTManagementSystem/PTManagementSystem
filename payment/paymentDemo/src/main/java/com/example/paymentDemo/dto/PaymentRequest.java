package com.example.paymentDemo.dto;

import lombok.NoArgsConstructor;
import lombok.Data;

@Data
@NoArgsConstructor
public class PaymentRequest {
    private String userId;
    private double amount;
    private String method;
    private String billingDetails;
    private Long appointmentId;

    public PaymentRequest(String userId, double amount, String method, String billingDetails, Long appointmentId) {
        this.userId = userId;
        this.amount = amount;
        this.method = method;
        this.billingDetails = billingDetails;
        this.appointmentId = appointmentId;
    }
    // Getters and setters
    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public double getAmount() {
        return amount;
    }

    public void setAmount(double amount) {
        this.amount = amount;
    }

    public String getMethod() {
        return method;
    }

    public void setMethod(String method) {
        this.method = method;
    }
    public String getBillingDetails() {
        return billingDetails;
    }
    public void setBillingDetails(String billingDetails) {
        this.billingDetails = billingDetails;
    }
    public Long getAppointmentId() {
        return appointmentId;
    }
    public void setAppointmentId(Long appointmentId) {
        this.appointmentId = appointmentId;
    }
}
