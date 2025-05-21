package com.example.paymentDemo.event;

import java.io.Serializable;
import java.time.LocalDateTime;

import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;

public class PaymentFailedEvent implements Serializable {
    private String reservationId;
    private String customerId;

    private Long paymentId;
    private LocalDateTime timestamp;
    private PaymentStatus status;
    private boolean isSuccess;

    public PaymentFailedEvent(Payment payment) {
        this.paymentId = payment.getId();
        this.status = payment.getStatus();
        this.timestamp = LocalDateTime.now();
    } 

    public boolean isSuccess() {
        return isSuccess;
    }

    public void setSuccess(boolean isSuccess) {
        this.isSuccess = isSuccess;
    }
    public Long getPaymentId() {
        return paymentId;
    }
    public void setPaymentId(Long paymentId) {
        this.paymentId = paymentId;
    }

    public LocalDateTime getTimestamp() {
        return timestamp;
    }
    public void setTimestamp(LocalDateTime timestamp) {
        this.timestamp = timestamp;
    }

    public String getReservationId() {
        return reservationId;
    }

    public void setReservationId(String reservationId) {
        this.reservationId = reservationId;
    }

    public String getCustomerId() {
        return customerId;
    }

    public void setCustomerId(String customerId) {
        this.customerId = customerId;
    }
    public PaymentStatus getStatus() {
        return status;
    }

    public void setStatus(PaymentStatus status) {
        this.status = status;
    }
}
