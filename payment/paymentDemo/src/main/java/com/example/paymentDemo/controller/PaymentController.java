package com.example.paymentDemo.controller;

import com.example.paymentDemo.service.PaymentService;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/payment")
public class PaymentController {

    private final PaymentService paymentService;

    public PaymentController(PaymentService paymentService) {
        this.paymentService = paymentService;
    }

    @PostMapping("/send")
    public String sendMessage(@RequestBody String message) {
        paymentService.sendPaymentMessage(message);
        return "Message sent: " + message;
    }
}
