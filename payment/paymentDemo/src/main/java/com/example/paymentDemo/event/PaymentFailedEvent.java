package com.example.paymentDemo.event;

import java.io.Serializable;

public class PaymentFailedEvent implements Serializable {
    private String reservationId;
    private String customerId;

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
