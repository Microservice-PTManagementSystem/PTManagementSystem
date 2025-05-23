package com.example.paymentDemo.dto;

import com.example.paymentDemo.dto.PaymentInfoDto;
import com.fasterxml.jackson.annotation.JsonProperty;

public class PaymentInfoSentEvent {
    @JsonProperty("user_id")
    private String userId;
    @JsonProperty("paymentInfo")
    private PaymentInfoDto paymentInfo;

    public PaymentInfoSentEvent() {
    }

    public PaymentInfoSentEvent(String userId, PaymentInfoDto paymentInfo) {
        this.userId = userId;
        this.paymentInfo = paymentInfo;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public PaymentInfoDto getPaymentInfo() {
        return paymentInfo;
    }

    public void setPaymentInfo(PaymentInfoDto paymentInfo) {
        this.paymentInfo = paymentInfo;
    }
}
