package com.example.paymentDemo.event;

import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;

import com.fasterxml.jackson.annotation.JsonProperty;

import lombok.Getter;
import lombok.Setter;

@Data
@NoArgsConstructor
@Getter
@Setter
public class PaymentProcessEvent implements Serializable {
    private String appointmentId;
    @JsonProperty("user_id")
    private String userId;
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
    @JsonProperty("slot_id")
    private String slotId;

    // Constructor for creating from appointment service data
    public PaymentProcessEvent(String appointmentId, String userId, String paymentMethod, 
                             String cardNumber, String cardHolder, String expiryMonth, 
                             String expiryYear, String cvc, boolean saveCard, 
                             String totalAmount, String slotId) {
        this.appointmentId = appointmentId;
        this.userId = userId;
        this.paymentMethod = paymentMethod;
        this.cardNumber = cardNumber;
        this.cardHolder = cardHolder;
        this.expiryMonth = expiryMonth;
        this.expiryYear = expiryYear;
        this.cvc = cvc;
        this.saveCard = saveCard;
        this.totalAmount = totalAmount;
        this.slotId = slotId;
    }
} 