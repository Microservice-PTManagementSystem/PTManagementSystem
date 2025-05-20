package com.example.paymentDemo.controller;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import com.example.paymentDemo.service.PaymentService;
import com.example.paymentDemo.repository.PaymentRepository;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
//import org.springframework.security.core.annotation.AuthenticationPrincipal;
//import org.springframework.security.oauth2.jwt.Jwt;

import java.util.*;

@RestController
@RequestMapping("/payment")
@CrossOrigin(origins = "*")
public class PaymentController {

    private final PaymentService paymentService;
    private final PaymentRepository paymentRepository;

    public PaymentController(PaymentService paymentService, PaymentRepository paymentRepository) { 
        this.paymentService = paymentService;
        this.paymentRepository = paymentRepository; }


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
    @PostMapping("/api/paymentConfirm")
public ResponseEntity<Payment> handleFrontendPayment(@RequestBody Map<String, Object> payload) {
    String method = (String) payload.get("paymentMethod");
    String cardNumber = (String) payload.get("cardNumber");
    String cardHolder = (String) payload.get("cardHolder");
    String expiryMonth = (String) payload.get("expiryMonth");
    String expiryYear = (String) payload.get("expiryYear");
    String cvc = (String) payload.get("cvc");
    boolean saveCard = Boolean.parseBoolean(payload.get("saveCard").toString());
    double amount = Double.parseDouble(payload.get("totalAmount").toString());

    // Örnek sabit değerler (geliştirme sırasında), sonra gerçek verilerle değiştirilmeli
    String userId = "user-frontend"; // frontend'den alınması önerilir
    Long appointmentId = 1L; // frontend'e eklenebilir

    // PaymentRequest oluştur
    PaymentRequest request = new PaymentRequest(method,cardNumber,cardHolder,expiryMonth,expiryYear,cvc,saveCard,String.valueOf(amount));

    // İşleme başlat
    Payment payment = paymentService.initiatePayment(request);

    // Varsayalım işlem başarılı (demo için)
    PaymentResult result = new PaymentResult();
    result.setPaymentId(payment.getId()); 

    // Onayla ve sonucu dön
    return ResponseEntity.ok(paymentService.confirmPayment(result));
}

    /*
    @GetMapping("/user")
    public ResponseEntity<List<Payment>> getPaymentsForUser(@AuthenticationPrincipal Jwt jwt) {
    String userId = jwt.getClaimAsString("sub"); // Keycloak'tan sub ID
    List<Payment> payments = paymentService.getPaymentsByUserId(userId);
    return ResponseEntity.ok(payments);
    }*/
   /* @GetMapping("/{paymentId}")
public ResponseEntity<Payment> getPaymentById(@PathVariable Long paymentId) {
    return ResponseEntity.ok(
        paymentRepository.findById(paymentId)
            .orElseThrow(() -> new IllegalArgumentException("Payment not found"))
    );
}
@GetMapping("/status")
public ResponseEntity<List<Payment>> getPaymentsByStatus(@RequestParam PaymentStatus status) {
    return ResponseEntity.ok(paymentRepository.findByStatus(status));
}
*/


    
}
