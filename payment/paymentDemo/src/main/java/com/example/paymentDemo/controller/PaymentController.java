package com.example.paymentDemo.controller;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.service.PaymentService;
import com.example.paymentDemo.repository.PaymentRepository;

import io.swagger.v3.oas.annotations.tags.Tag;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
//import org.springframework.security.core.annotation.AuthenticationPrincipal;
//import org.springframework.security.oauth2.jwt.Jwt;

import java.util.*;

import org.springframework.amqp.rabbit.core.RabbitTemplate;

@RestController
@RequestMapping("/payment")
//@CrossOrigin(origins = "*")
@Tag(name = "Payment API", description = "Ödeme işlemleri için endpointler")
public class PaymentController {

    private final PaymentService paymentService;
    private final PaymentRepository paymentRepository;
    private final RabbitTemplate rabbitTemplate;

    public PaymentController(PaymentService paymentService, PaymentRepository paymentRepository, RabbitTemplate rabbitTemplate) { 
        this.paymentService = paymentService;
        this.paymentRepository = paymentRepository;
        this.rabbitTemplate = rabbitTemplate; }


    @GetMapping("/payment/health")
    public String checkHealth() {
        return "Payment Service is running!";
    } 

    @PostMapping("/initiate")
    public ResponseEntity<Payment> initiatePayment(@RequestBody PaymentRequest request) {
        Payment payment = paymentService.initiatePayment(request);
        return ResponseEntity.ok(payment);
    } //bunu düzelt

    @PostMapping("/confirm")
    public ResponseEntity<Payment> confirmPayment(@RequestBody PaymentResult result) {
        Payment payment = paymentService.confirmPayment(result);
        PaymentResult response = new PaymentResult();
        response.setSuccess(payment.getStatus() == PaymentStatus.COMPLETED);
        //response.setMessage(payment.getStatus() == PaymentStatus.COMPLETED ? 
         //   "Payment succeeded" : "Payment failed");
        return ResponseEntity.ok(payment);
    }

    @PostMapping("/retry")
    public ResponseEntity<Payment> retryPayment(@RequestParam String slotId) {
        Payment payment = paymentService.retryPayment(slotId);
        return ResponseEntity.ok(payment);
    }

    @PostMapping("/refund")
    public ResponseEntity<Payment> issueRefund(@RequestParam String slotId) {
        Payment payment = paymentService.issueRefund(slotId);
        return ResponseEntity.ok(payment);
    }
    @GetMapping("/status")
public ResponseEntity<PaymentStatus> getStatus(@RequestParam String slotId) {
    try {
        PaymentStatus status = paymentService.getStatus(slotId);
        return ResponseEntity.ok(status);
    } catch (IllegalArgumentException e) {
        return ResponseEntity.notFound().build();
    }
    }
    @GetMapping("/getSavedCard")
    public PaymentRequest getSavedCard(@RequestParam String userId) {
        PaymentRequest paymentRequest = (PaymentRequest) rabbitTemplate.convertSendAndReceive(
            "user.exchange",    // exchange
            "user.getSavedCard",// routing key
            userId              // mesaj içeriği
        );

        if(paymentRequest == null) {
            throw new RuntimeException("Kart bilgisi bulunamadı");
        }

        return paymentRequest;
    }
    
}