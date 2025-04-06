// AppointmentEventListener.java
package com.example.listener;

import com.example.events.appointment.AppointmentCanceledEvent;
import com.example.events.appointment.SlotConfirmedEvent;
import com.example.service.PaymentService;
import lombok.RequiredArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

@Component
@RequiredArgsConstructor
public class AppointmentEventListener {
    private final PaymentService paymentService;

    @RabbitListener(queues = "appointment.slot.confirmed.queue")
    public void handleSlotConfirmed(SlotConfirmedEvent event) {
        paymentService.handleSlotConfirmed(
            event.getAppointmentId(),
            event.getUserId(),
            event.getAmount()
        );
    }

    @RabbitListener(queues = "appointment.canceled.queue")
    public void handleAppointmentCanceled(AppointmentCanceledEvent event) {
        paymentService.handleAppointmentCanceled(event.getAppointmentId());
    }
}