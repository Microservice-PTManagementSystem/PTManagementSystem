package com.example.paymentDemo.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

import lombok.Data;
@Data
public class PaymentRequest {
    @JsonProperty("paymentMethod")
    private String paymentMethod;
    @JsonProperty("cardNumber")
    private String cardNumber;
    @JsonProperty("cardHolder")
    private String cardHolder;
    @JsonProperty("expiryMonth")
    private String expiryMonth;
    @JsonProperty("expiryYear")
    private String expiryYear;
    @JsonProperty("cvc")
    private String cvc;
    @JsonProperty("saveCard")
    private boolean saveCard;
    @JsonProperty("totalAmount")
    private String totalAmount;
    private String slotId;
    private String userId;

    public PaymentRequest() {}
    public PaymentRequest(String paymentMethod, String cardNumber, String cardHolder, String expiryMonth, String expiryYear, String cvc, String totalAmount) {
        this.paymentMethod = paymentMethod;
        this.cardNumber = cardNumber;
        this.cardHolder = cardHolder;
        this.expiryMonth = expiryMonth;
        this.expiryYear = expiryYear;
        this.cvc = cvc;
        this.totalAmount = totalAmount;
    }  
    public String getSlotId() {
        return slotId;
    }
    public void setSlotId(String slotId) {
        this.slotId = slotId;
    }

    public String getPaymentMethod() {
        return paymentMethod;
    }
    public void setPaymentMethod(String paymentMethod) {
        this.paymentMethod = paymentMethod;
    }
    public String getCardNumber() {
        return cardNumber;
    }
    public void setCardNumber(String cardNumber) {
        this.cardNumber = cardNumber;
    }
    public String getCardHolder() {
        return cardHolder;
    }
    public void setCardHolder(String cardHolder) {
        this.cardHolder = cardHolder;
    }
    public String getExpiryMonth() {
        return expiryMonth;
    }
    public void setExpiryMonth(String expiryMonth) {
        this.expiryMonth = expiryMonth;
    }
    public String getExpiryYear() {
        return expiryYear;
    }   
    public void setExpiryYear(String expiryYear) {
        this.expiryYear = expiryYear;
    }
    public String getCvc() {
        return cvc;
    }
    public void setCvc(String cvc) {
        this.cvc = cvc;
    }
    public boolean isSaveCard() {
        return saveCard;
    }
    public void setSaveCard(boolean saveCard) {
        this.saveCard = saveCard;
    }
    public String getTotalAmount() {
        return totalAmount;
    }
    public void setTotalAmount(String totalAmount) {
        this.totalAmount = totalAmount;
        
    }
    public String getUserId() {
        return userId;
    }
    public void setUserId(String userId) {
        this.userId = userId;
    }
}