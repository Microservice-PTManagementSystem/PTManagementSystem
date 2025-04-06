// AppointmentCanceledEvent.java
package com.example.events.appointment;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class AppointmentCanceledEvent {
    private String appointmentId;
    private String userId;
    private String trainerId;
    private String reason;
    private LocalDateTime timestamp;

    public AppointmentCanceledEvent() {
        this.timestamp = LocalDateTime.now();
    }
}