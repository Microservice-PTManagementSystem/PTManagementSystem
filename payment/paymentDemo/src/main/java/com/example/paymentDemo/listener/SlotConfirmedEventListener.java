package com.example.paymentDemo.listener;

import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Component;

import com.example.paymentDemo.event.SlotReservedEvent;
import com.example.paymentDemo.service.PaymentService; //paymentservice classını çektim aşağıya da ekledim sadece

@Component
public class SlotConfirmedEventListener {

    private final RabbitTemplate rabbitTemplate;
    private final PaymentService paymentService;

    public SlotConfirmedEventListener(RabbitTemplate rabbitTemplate,PaymentService paymentService) {
        this.rabbitTemplate = rabbitTemplate;
        this.paymentService = paymentService;
        System.out.println("Slot Confirmed Initiated");
    }

    @RabbitListener(queues = "reservationQueue")
    public void handleSlotConfirmed(SlotReservedEvent event) {
        System.out.println("✅ [PAYMENT] Mesaj alındı: " + event);

        // Gerekirse yeni bir event oluştur, ya da geleni kullan
       /* SlotReservedEvent slotReserved = new SlotReservedEvent(
            event.getAppointmentDate(),
            event.getAppointmentTime(),
            event.getCreatedDate(),
            event.getTrainerId(),
            event.getCustomerId(),
            event.getId()
        );*/

        paymentService.processPayment(event);
        rabbitTemplate.convertAndSend("pt.topic.exchange", "slot.reserved", event);
}
        //rabbitTemplate.convertAndSend("SlotReservedEvent","pt.topic.exchange", slotReserved);
        //System.out.println("SlotReservedEvent Published! " + slotReserved);
    }



