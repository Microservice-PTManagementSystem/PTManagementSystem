// PaymentService.java
package com.example.service;

import com.example.dto.PaymentRequest;
import com.example.dto.PaymentResponse;
import com.example.domain.Payment;

public interface PaymentService {
    PaymentResponse initiatePayment(PaymentRequest request);
    PaymentResponse processPayment(String paymentId);
    PaymentResponse getPaymentDetails(String paymentId);
    PaymentResponse refundPayment(String paymentId);
    void handleSlotConfirmed(String appointmentId, String userId, Double amount);
    void handleAppointmentCanceled(String appointmentId);
}