package com.example.paymentDemo.service;

import com.example.paymentDemo.dto.PaymentRequest;
import com.example.paymentDemo.dto.PaymentResult;
import com.example.paymentDemo.model.Payment;

import java.util.*;

import com.example.paymentDemo.model.PaymentStatus;

public interface PaymentService {
    Payment initiatePayment(PaymentRequest request);
    Payment confirmPayment(PaymentResult result);
    Payment retryPayment(Long paymentId);
    Payment issueRefund(Long paymentId);
    PaymentStatus getStatus(Long paymentId);
}
