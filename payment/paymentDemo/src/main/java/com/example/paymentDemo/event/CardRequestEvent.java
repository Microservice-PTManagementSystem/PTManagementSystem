package com.example.paymentDemo.event;

import com.fasterxml.jackson.annotation.JsonProperty;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class CardRequestEvent {
    @JsonProperty("user_id")
    private String userId;
    @JsonProperty("useSavedCard")
    private boolean useSavedCard;

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public boolean isUseSavedCard() {
        return useSavedCard;
    }

    public void setUseSavedCard(boolean useSavedCard) {
        this.useSavedCard = useSavedCard;
    }
    @Override
    public String toString() {
        return "CardRequestEvent{" +
                "userId='" + userId + '\'' +
                ", useSavedCard=" + useSavedCard +
                '}';
    }
}