package com.example.paymentDemo.repository;

import com.example.paymentDemo.model.Payment;
import com.example.paymentDemo.model.PaymentStatus;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
 
import java.util.*;

@Repository
public interface PaymentRepository extends JpaRepository<Payment, Long> {

    // Payment'i userId ve appointmentId ile sorgulama
    Optional<Payment> findByUserIdAndAppointmentId(String userId, Long appointmentId);
    
    // Ödeme durumu ile ödeme sorgulama
    List<Payment> findByStatus(PaymentStatus status);

    // Payment Id ile ödeme arama
    Optional<Payment> findById(Long id);
    //List<Payment> findByUserId(String userId);

}
 