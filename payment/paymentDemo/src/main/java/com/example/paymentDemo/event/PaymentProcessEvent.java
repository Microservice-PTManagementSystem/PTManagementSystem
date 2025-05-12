package com.example.paymentDemo.event;

import lombok.Data;
import lombok.NoArgsConstructor;
import java.io.Serializable;

@Data
@NoArgsConstructor
public class PaymentProcessEvent implements Serializable {
    private String appointmentId;
    private String userId;
    private String paymentMethod;
    private String cardNumber;
    private String cardHolder;
    private String expiryMonth;
    private String expiryYear;
    private String cvc;
    private boolean saveCard;
    private String totalAmount;
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