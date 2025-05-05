package com.example.paymentDemo.dto;

import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * DTO representing the result of a payment attempt.
 */
@Data
@NoArgsConstructor
public class PaymentResult {
 
    private Long paymentId;
    private boolean success;
    private String transactionReference; // e.g., credit card transaction ID
    private String failureReason;

    public PaymentResult(Long paymentId, boolean success, String transactionReference, String failureReason) {
        this.paymentId = paymentId;
        this.success= success;
        this.transactionReference = "";
        this.failureReason = "";
    }

    public boolean isSuccess() {
        return success;
    }
    public void setSuccess(boolean success) {
        this.success = success;
    }
    public String getTransactionReference() {
        return transactionReference;
    }
    public void setTransactionReference(String transactionReference) {
        this.transactionReference = transactionReference;
    }
    public String getFailureReason() {
        return failureReason;
    }
    public void setFailureReason(String failureReason) {
        this.failureReason = failureReason;
    }
    public Long getPaymentId() {
        return paymentId;
    }
    public void setPaymentId(Long paymentId) {
        this.paymentId = paymentId;
    }
    
}
