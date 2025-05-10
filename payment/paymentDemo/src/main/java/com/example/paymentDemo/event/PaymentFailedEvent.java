package com.example.paymentDemo.event;

import java.io.Serializable;
import java.time.LocalDateTime;

import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;

public class PaymentFailedEvent implements Serializable {
    private String reservationId;
    private String customerId;

    private Long paymentId;
    private String failureReason;
    private LocalDateTime timestamp;
    private PaymentStatus status;

    public PaymentFailedEvent(Payment payment) {
        this.paymentId = payment.getId();
        this.failureReason = payment.getFailureReason(); // örnek alan
        this.timestamp = LocalDateTime.now();
        this.status = payment.getStatus();
    } 

    public Long getPaymentId() {
        return paymentId;
    }
    public void setPaymentId(Long paymentId) {
        this.paymentId = paymentId;
    }
    public String getFailureReason() {
        return failureReason;
    }
    public void setFailureReason(String failureReason) {
        this.failureReason = failureReason;
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
}
