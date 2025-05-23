package com.example.paymentDemo.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class CardRequest{
    @JsonProperty("user_id")
    private String userId;
    @JsonProperty("slot_id")
    private String slotId;

    public CardRequest(String userId, String slotId){
        this.userId=userId;
        this.slotId=slotId;
    }
  public String getUserId(){
    return userId;
  }
  public String getSlotId(){
    return slotId;
  }
  public void setUserId(String userId) {
        this.userId = userId;
    }
    public void setSlotId(String slotId) {
        this.slotId = slotId;
    }
}