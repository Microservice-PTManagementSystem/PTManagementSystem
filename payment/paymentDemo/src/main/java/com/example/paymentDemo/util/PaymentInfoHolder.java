package com.example.paymentDemo.util;

import java.util.Map;
import java.util.concurrent.*;

import org.springframework.stereotype.Component;

import com.example.paymentDemo.dto.PaymentInfoDto;

@Component
public class PaymentInfoHolder {

    private final Map<String, CompletableFuture<PaymentInfoDto>> futures = new ConcurrentHashMap<>();

    public CompletableFuture<PaymentInfoDto> createFuture(String userId) {
        CompletableFuture<PaymentInfoDto> future = new CompletableFuture<>();
        futures.put(userId, future);
        return future;
    }

    public void complete(String userId, PaymentInfoDto paymentInfo) {
        CompletableFuture<PaymentInfoDto> future = futures.remove(userId);
        if (future != null) {
            future.complete(paymentInfo);
        }
    }
}
