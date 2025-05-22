package com.example.paymentDemo.event;
import com.example.paymentDemo.dto.PaymentInfoDto;

public class PaymentInfoSentEvent {
    private String userId;
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