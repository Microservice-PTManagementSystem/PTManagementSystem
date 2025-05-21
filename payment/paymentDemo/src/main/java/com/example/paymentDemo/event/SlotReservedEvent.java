package com.example.paymentDemo.event;

import java.io.Serializable;

import com.fasterxml.jackson.annotation.JsonProperty;

public class SlotReservedEvent {
    @JsonProperty("slot_id")
    private String slotId;

    @JsonProperty("user_id")
    private String userId;

    @JsonProperty("timestamp")
    private String timestamp;

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
    public Object Payment;
    private boolean savedCardUse;
    private boolean success;
    

    public SlotReservedEvent() {
    }   

    public SlotReservedEvent(String slotId,String userId,String timestamp,String paymentMethod,
    String cardNumber,String cardHolder,String expiryMonth,String expiryYear,String cvc,boolean saveCard,String totalAmount  ) {
        this.userId =userId;
        this.timestamp = timestamp;
        this.slotId=slotId;
        this.paymentMethod=paymentMethod;
        this.cardNumber=cardNumber;
        this.cardHolder=cardHolder;
        this.expiryMonth=expiryMonth;
        this.expiryYear=expiryYear;
        this.cvc=cvc;
        this.saveCard=saveCard;
        this.totalAmount=totalAmount;


    }

    

    // Getters and Setters
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

    public String getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(String timestamp) {
        this.timestamp = timestamp;
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

    public void setSavedCard(boolean saveCard) {
        this.saveCard = saveCard;
    }  

    public boolean isSavedCardUse() {
        return savedCardUse;
    }
    public void setSaveCardUse(boolean savedCardUse) {
        this.savedCardUse = savedCardUse;
    } 

    public String getTotalAmount() {
        return totalAmount;
    }

    public void setTotalAmount(String totalAmount) {
        this.totalAmount = totalAmount;
    }

    public boolean success(boolean success){
        return true;
    }
    

}