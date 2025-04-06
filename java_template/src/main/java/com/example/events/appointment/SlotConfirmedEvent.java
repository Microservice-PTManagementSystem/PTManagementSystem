// SlotConfirmedEvent.java
package com.example.events.appointment;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class SlotConfirmedEvent {
    private String appointmentId;
    private String userId;
    private String trainerId;
    private Double amount;
    private LocalDateTime appointmentTime;
    private LocalDateTime timestamp;

    public SlotConfirmedEvent() {
        this.timestamp = LocalDateTime.now();
    }
}