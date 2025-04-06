// PaymentController.java
package com.example.controller;

import com.example.dto.PaymentRequest;
import com.example.dto.PaymentResponse;
import com.example.service.PaymentService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/payments")
@RequiredArgsConstructor
@Api(tags = "Payment Management")
public class PaymentController {
    private final PaymentService paymentService;

    @PostMapping
    @ApiOperation("Initiate a new payment")
    public ResponseEntity<PaymentResponse> initiatePayment(@RequestBody PaymentRequest request) {
        return ResponseEntity.ok(paymentService.initiatePayment(request));
    }

    @PostMapping("/{paymentId}/process")
    @ApiOperation("Process a pending payment")
    public ResponseEntity<PaymentResponse> processPayment(@PathVariable String paymentId) {
        return ResponseEntity.ok(paymentService.processPayment(paymentId));
    }

    @GetMapping("/{paymentId}")
    @ApiOperation("Get payment details")
    public ResponseEntity<PaymentResponse> getPayment(@PathVariable String paymentId) {
        return ResponseEntity.ok(paymentService.getPaymentDetails(paymentId));
    }

    @PostMapping("/{paymentId}/refund")
    @ApiOperation("Refund a payment")
    public ResponseEntity<PaymentResponse> refundPayment(@PathVariable String paymentId) {
        return ResponseEntity.ok(paymentService.refundPayment(paymentId));
    }
}