// PaymentResponse.java
package com.example.dto;

import com.example.domain.PaymentStatus;
import lombok.Data;
import java.time.LocalDateTime;

@Data
public class PaymentResponse {
    private String paymentId;
    private PaymentStatus status;
    private Double amount;
    private String transactionId;
    private LocalDateTime createdAt;
    private LocalDateTime updatedAt;
}