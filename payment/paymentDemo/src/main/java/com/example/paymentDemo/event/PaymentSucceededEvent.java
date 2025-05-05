package com.example.paymentDemo.event;

import java.io.Serializable;

public class PaymentSucceededEvent {
    private String slotId;
    private String trainerId;
    private String customerId;

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
}
