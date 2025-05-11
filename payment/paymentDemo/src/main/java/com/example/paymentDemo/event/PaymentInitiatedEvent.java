package com.example.paymentDemo.event;

import com.example.paymentDemo.model.Payment;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class PaymentInitiatedEvent {
    private String appointmentId;
    private String userId;
    private Payment saved;
    private String paymentId;

    private Long slotId;
    private String paymentMethod;
    private String cardNumber;
    private String cardHolder;
    private String expiryMonth;
    private String expiryYear;
    private String cvc;
    private boolean saveCard;
    private String totalAmount;
    // getters and setters

    public PaymentInitiatedEvent(Payment saved) {
        this.saved = saved;
        this.userId = String.valueOf(saved.getUserId());
        this.totalAmount = saved.getTotalAmount();
        this.appointmentId = String.valueOf(saved.getAppointmentId());
    } 
    
    public String getAppointmentId() {
        return appointmentId;
    }

    public void setAppointmentId(String appointmentId) {
        this.appointmentId = appointmentId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getTotalAmount() {
        return totalAmount;
    }

    public void setTotalAmount(String totalAmount) {
        this.totalAmount = totalAmount;
    }
    public String getPaymentId() {
        return paymentId;
    }

    public void setPaymentId(String paymentId) {
        this.paymentId = paymentId;
    }
    public String getPaymentMethod() {
        return paymentMethod;
    }

    public void setPaymentMethod(String paymentMethod) {
        this.paymentMethod = paymentMethod;
    }
}
