package com.example.paymentDemo.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class PaymentController {

    @GetMapping("/payment/health")
    public String checkHealth() {
        return "Payment Service is running!";
    }
}

