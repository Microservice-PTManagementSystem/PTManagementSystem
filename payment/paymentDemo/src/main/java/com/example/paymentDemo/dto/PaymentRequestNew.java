package com.example.paymentDemo.dto;
import lombok.Data;
import java.io.Serializable;
import com.fasterxml.jackson.annotation.JsonProperty;

@Data

public class PaymentRequestNew {
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
    @JsonProperty("hourly_price")
    private String hourlyPrice;
    @JsonProperty("slot_id")
    private String slotId;
    @JsonProperty("user_id")
    private String userId;

    public PaymentRequestNew() {
    }
    public PaymentRequestNew(String userId,String slotId,String paymentMethod, String cardNumber, String cardHolder, String expiryMonth, String expiryYear, String cvc, String hourlyPrice) {
        this.userId = userId;
        this.slotId = slotId;
        this.paymentMethod = paymentMethod;
        this.cardNumber = cardNumber;
        this.cardHolder = cardHolder;
        this.expiryMonth = expiryMonth;
        this.expiryYear = expiryYear;
        this.cvc = cvc;
        this.hourlyPrice = hourlyPrice;
    }
    public String getUserId() {
        return userId;
    }
    public void setUserId(String userId) {
        this.userId = userId;
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
    public String getCardNumber() {
        return cardNumber;
    }
    public String getCardHolder() {
        return cardHolder;
    }
    public String getExpiryMonth() {
        return expiryMonth;
    }
    public String getExpiryYear() {
        return expiryYear;
    }
    public String getCvc() {
        return cvc;
    }
    public String getHourlyPrice() {
        return hourlyPrice;
    }
    public void setHourlyPrice(String hourlyPrice) {
        this.hourlyPrice = hourlyPrice;
    }
    public void setPaymentMethod(String paymentMethod) {
        this.paymentMethod = paymentMethod;
    }
    public void setCardNumber(String cardNumber) {
        this.cardNumber = cardNumber;
    }
    public void setCardHolder(String cardHolder) {
        this.cardHolder = cardHolder;
    }
    public void setExpiryMonth(String expiryMonth) {
        this.expiryMonth = expiryMonth;
    }
    public void setExpiryYear(String expiryYear) {
        this.expiryYear = expiryYear;
    }
    public void setCvc(String cvc) {
        this.cvc = cvc;
    }

    // Getters ve setters ya da Lombok kullanabilirsin.
}

