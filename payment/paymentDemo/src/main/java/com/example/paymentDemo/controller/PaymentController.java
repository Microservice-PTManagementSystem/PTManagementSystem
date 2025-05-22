package com.example.paymentDemo.controller;

import com.example.paymentDemo.dto.*;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.event.*;
import com.example.paymentDemo.service.PaymentService;
import com.example.paymentDemo.repository.PaymentRepository;

import io.swagger.v3.oas.annotations.tags.Tag;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
//import org.springframework.security.core.annotation.AuthenticationPrincipal;
//import org.springframework.security.oauth2.jwt.Jwt;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.http.HttpStatus;

import com.example.paymentDemo.dto.ConfirmRequest;
import com.example.paymentDemo.dto.InitiateResponse;
import com.example.paymentDemo.dto.PaymentRequestNew;
import com.example.paymentDemo.dto.PaymentResponse;
import com.example.paymentDemo.event.CardRequestEvent;

@RestController
@RequestMapping("/payment")
//@CrossOrigin(origins = "*")
@Tag(name = "Payment API", description = "Ödeme işlemleri için endpointler")
public class PaymentController {
    private final Map<String, PaymentStatus> paymentStatusMap = new ConcurrentHashMap<>();
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
/*
    @PostMapping("/initiate")
    public ResponseEntity<Payment> initiatePayment(@RequestBody PaymentRequest request) {
        Payment payment = paymentService.initiatePayment(request);
        return ResponseEntity.ok(payment);
    } */
   @PostMapping("/initiate")
public ResponseEntity<InitiateResponse> initiatePayment(@RequestBody PaymentRequestNew request) {
    // basit bir validasyon örneği
    if (!request.getCvc().matches("\\d{3}")) {
        return ResponseEntity.badRequest().body(
            new InitiateResponse(PaymentStatus.FAILED, "CVC must be exactly 3 digits.",
                                        request.getSlotId(), request.getUserId())
        );
    }
    paymentStatusMap.put(request.getSlotId(), PaymentStatus.COMPLETED);

    return ResponseEntity.ok(
        new InitiateResponse(PaymentStatus.COMPLETED, "Payment initiated successfully.",
                                    request.getSlotId(), request.getUserId())
    );
}

    @PostMapping("/confirm")
    public ResponseEntity<PaymentResponse> confirmPayment(@RequestBody ConfirmRequest request) {
        boolean isPaymentSuccessful = !request.getCardNumber().endsWith("0");

        if (!request.getCvc().matches("\\d{3}")) {
            return ResponseEntity.badRequest().body(
                new PaymentResponse(PaymentStatus.FAILED, "CVV must be exactly 3 digits.")
            );
        }

        // Eğer geçerliyse başarılı döner
        return ResponseEntity.ok(
            new PaymentResponse(PaymentStatus.COMPLETED, "Payment confirmed successfully.")
        );
    
    }

    @PostMapping("/retry")
    public ResponseEntity<PaymentResponse> retryPayment(@RequestBody ConfirmRequest request) {
        String slotId = request.getSlotId();
        PaymentStatus currentStatus = paymentStatusMap.get(slotId);

    if (currentStatus == null) {
        return ResponseEntity.badRequest().body(
            new PaymentResponse(PaymentStatus.FAILED, "No payment attempt found for slotId: " + slotId)
        );
    }

    if (currentStatus == PaymentStatus.COMPLETED) {
        return ResponseEntity.badRequest().body(
            new PaymentResponse(PaymentStatus.FAILED, "Payment already completed for slotId: " + slotId)
        );
    }

    if (!request.getCvc().matches("\\d{3}")) {
        return ResponseEntity.badRequest().body(
            new PaymentResponse(PaymentStatus.FAILED, "Retry failed: CVV must be exactly 3 digits.")
        );
    }

    paymentStatusMap.put(slotId, PaymentStatus.COMPLETED);
    return ResponseEntity.ok(
        new PaymentResponse(PaymentStatus.COMPLETED, "Retry succeeded: Payment confirmed.")
    );
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
   @PostMapping("/getSavedCard")
public ResponseEntity<String> isUseSavedCard(@RequestBody CardRequestEvent request) {

    System.out.println("GÖNDERİLEN EVENT: " + request);
    
    rabbitTemplate.convertAndSend(
        "user.exchange",
        "user.getSavedCard",
        request
    );

    if (request.isUseSavedCard()) {
        return ResponseEntity.ok("success");
    } else {
        return ResponseEntity.ok("fail");
    }
}


}
    
