package com.example.paymentDemo.controller;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.service.PaymentService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/payment")
public class PaymentController {

    private final PaymentService paymentService;
    public PaymentController(PaymentService paymentService) { 
        this.paymentService = paymentService; }


    @GetMapping("/payment/health")
    public String checkHealth() {
        return "Payment Service is running!";
    }

    @PostMapping("/initiate")
    public ResponseEntity<Payment> initiatePayment(@RequestBody PaymentRequest request) {
        Payment payment = paymentService.initiatePayment(request);
        return ResponseEntity.ok(payment);
    }

    @PostMapping("/confirm")
    public ResponseEntity<Payment> confirmPayment(@RequestBody PaymentResult result) {
        Payment payment = paymentService.confirmPayment(result);
        return ResponseEntity.ok(payment);
    }

    @PostMapping("/retry/{paymentId}")
    public ResponseEntity<Payment> retryPayment(@PathVariable Long paymentId) {
        Payment payment = paymentService.retryPayment(paymentId);
        return ResponseEntity.ok(payment);
    }

    @PostMapping("/refund/{paymentId}")
    public ResponseEntity<Payment> issueRefund(@PathVariable Long paymentId) {
        Payment payment = paymentService.issueRefund(paymentId);
        return ResponseEntity.ok(payment);
    }
    @GetMapping("/status/{id}")
public ResponseEntity<PaymentStatus> getStatus(@PathVariable Long id) {
    try {
        PaymentStatus status = paymentService.getStatus(id);
        return ResponseEntity.ok(status);
    } catch (IllegalArgumentException e) {
        return ResponseEntity.notFound().build();
    }
}

    
}

