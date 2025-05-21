package com.example.paymentDemo.service;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.model.Payment;

import java.util.*;

import com.example.paymentDemo.model.PaymentStatus;
import com.fasterxml.jackson.databind.ser.impl.StringArraySerializer;

public interface PaymentService {
    Payment initiatePayment(PaymentRequest request);
    Payment confirmPayment(PaymentResult result);
    Payment retryPayment(String slotId);
    Payment issueRefund(String slotId);
    PaymentStatus getStatus(String slotId);
   // List<Payment> getPaymentsByUserId(String userId);
    PaymentResult processPayment(PaymentRequest request);
    PaymentRequest getSavedCardByUserId(String userId);

}
 