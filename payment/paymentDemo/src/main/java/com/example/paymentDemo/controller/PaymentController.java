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
    public ResponseEntity<PaymentResult> confirmPayment(@RequestBody PaymentResult result) {
        Payment payment = paymentService.confirmPayment(result);
        PaymentResult response = new PaymentResult();
        response.setSuccess(payment.getStatus() == PaymentStatus.COMPLETED);
        //response.setMessage(payment.getStatus() == PaymentStatus.COMPLETED ? 
         //   "Payment succeeded" : "Payment failed");
        return ResponseEntity.ok(response);
    }

    @PostMapping("/retry/{slotId}")
    public ResponseEntity<Payment> retryPayment(@PathVariable String slotId) {
        Payment payment = paymentService.retryPayment(slotId);
        return ResponseEntity.ok(payment);
    }

    @PostMapping("/refund/{slotId}")
    public ResponseEntity<Payment> issueRefund(@PathVariable String slotId) {
        Payment payment = paymentService.issueRefund(slotId);
        return ResponseEntity.ok(payment);
    }
    @GetMapping("/status/{id}")
public ResponseEntity<PaymentStatus> getStatus(@PathVariable String id) {
    try {
        PaymentStatus status = paymentService.getStatus(id);
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
    
    @PostMapping("/api/paymentConfirm")
public ResponseEntity<Payment> handleFrontendPayment(@RequestBody Map<String, Object> payload) {
    String paymentMethod = (String) payload.get("paymentMethod");
    String cardNumber = (String) payload.get("cardNumber");
    String cardHolder = (String) payload.get("cardHolder");
    String expiryMonth = (String) payload.get("expiryMonth");
    String expiryYear = (String) payload.get("expiryYear");
    String cvc = (String) payload.get("cvc");
    String totalAmount = (String) payload.get("totalAmount");

    // Örnek sabit değerler (geliştirme sırasında), sonra gerçek verilerle değiştirilmeli
    String userId = "user-frontend"; // frontend'den alınması önerilir

    // PaymentRequest oluştur
    PaymentRequest request = new PaymentRequest(paymentMethod, cardNumber, cardHolder, expiryMonth, expiryYear, cvc, totalAmount);

    // İşleme başlat
    Payment payment = paymentService.initiatePayment(request);

    // Varsayalım işlem başarılı (demo için)
    PaymentResult result = new PaymentResult();
    result.setPaymentId(payment.getId()); 

    // Onayla ve sonucu dön
    return ResponseEntity.ok(paymentService.confirmPayment(result));
}




    
}