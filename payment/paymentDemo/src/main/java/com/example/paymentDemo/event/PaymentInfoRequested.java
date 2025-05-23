package com.example.paymentDemo.event;

import com.fasterxml.jackson.annotation.JsonProperty;

import lombok.Data;

@Data
public class PaymentInfoRequested {
    @JsonProperty("user_id")
    private String userId;

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }
    
    public PaymentInfoRequested(String userId) {
        this.userId = userId;
    }

    
   /* @Override
    public String toString() {
        return "CardRequestEvent{" +
                "userId='" + userId + '\'' +
                ", useSavedCard=" + useSavedCard +
                '}';
    }*/
    

}