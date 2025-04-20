package com.example.paymentDemo.event;

import java.io.Serializable;

import com.fasterxml.jackson.annotation.JsonProperty;

public class SlotReservedEvent implements Serializable {
    @JsonProperty("slot_id")
    private String slotId;

    @JsonProperty("user_id")
    private String userId;

    @JsonProperty("timestamp")
    private String timestamp;
    //private String appointment_time;
    //private String appointment_date;
    //private String created_date;
    //private String trainer_id;
    //private String customer_id; bi de id için var eklenmiş

    public SlotReservedEvent() {
    }   

    public SlotReservedEvent(String slotId,String userId,String timestamp ) {
        this.userId =userId;
        this.timestamp = timestamp;
        this.slotId=slotId;


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
}
