package com.example.paymentDemo.service;

import com.example.paymentDemo.event.PaymentFailedEvent;
import com.example.paymentDemo.event.PaymentSucceededEvent;

import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.*;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.Base64;
import java.util.HashMap;
import java.util.Map;

import com.example.paymentDemo.event.SlotReservedEvent;
import org.springframework.http.HttpStatus;

@Service
public class IyzicoPaymentService {

    private final RestTemplate restTemplate = new RestTemplate();
    private final RabbitTemplate rabbitTemplate;

    @Value("${iyzico.api-key}")
    private String apiKey;

    @Value("${iyzico.secret-key}")
    private String secretKey;

    @Value("${iyzico.base-url}")
    private String baseUrl;

    public IyzicoPaymentService(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    public boolean processPayment(SlotReservedEvent event) {
        try {
            HttpHeaders headers = createHeaders();

            Map<String, Object> request = new HashMap<>();
            request.put("locale", "tr");
            request.put("conversationId", event.getSlotId());
            request.put("price", event.getTotalAmount());
            request.put("paidPrice", event.getTotalAmount());
            request.put("currency", "TRY");
            request.put("installment", 1);
            request.put("paymentChannel", "WEB");
            request.put("paymentGroup", "PRODUCT");

            // Kart bilgileri sandbox için sabittir
            Map<String, String> card = new HashMap<>();
            card.put("cardHolderName", event.getCardHolder());
            card.put("cardNumber", event.getCardNumber());
            card.put("expireMonth", event.getExpiryMonth());
            card.put("expireYear", event.getExpiryYear());
            card.put("cvc", event.getCvc());
            card.put("registerCard", event.isSaveCard() ? "1":"0");

            request.put("paymentCard", card);

            // Dummy buyer
            Map<String, String> buyer = new HashMap<>();
            buyer.put("id", event.getUserId());
            buyer.put("name", "Jane");
            buyer.put("surname", "Doe");
            buyer.put("gsmNumber", "+905350000000");
            buyer.put("email", "email@example.com");
            buyer.put("identityNumber", "11111111111");
            buyer.put("registrationAddress", "Some address");
            buyer.put("ip", "85.34.78.112");
            buyer.put("city", "Istanbul");
            buyer.put("country", "Turkey");

            request.put("buyer", buyer);

            // Dummy item
            Map<String, Object> item = new HashMap<>();
            item.put("id", "item1");
            item.put("name", "Slot Reservation");
            item.put("category1", "Healthcare");
            item.put("itemType", "VIRTUAL");
            item.put("price", event.getTotalAmount());

            request.put("basketItems", new Map[]{item});

            HttpEntity<Map<String, Object>> entity = new HttpEntity<>(request, headers);

            ResponseEntity<String> response = restTemplate.postForEntity(baseUrl + "/payment/auth", entity, String.class);

            return response.getStatusCode() == HttpStatus.OK && response.getBody() != null && response.getBody().contains("\"status\":\"success\"");
            /*if (response.getStatusCode() == HttpStatus.OK && response.getBody().contains("\"status\":\"success\"")) {
                PaymentSucceededEvent succeededEvent = new PaymentSucceededEvent(event.getSlotId(), event.getUserId());
                rabbitTemplate.convertAndSend("PaymentSucceededEvent", succeededEvent);
                System.out.println("Ödeme başarılı: " + succeededEvent);
            } else {
                PaymentFailedEvent failedEvent = new PaymentFailedEvent(event.getSlotId(), event.getUserId());
                rabbitTemplate.convertAndSend("PaymentFailedEvent", failedEvent);
                System.out.println("Ödeme başarısız: " + failedEvent);
            }*/

        } catch (Exception ex) {
            ex.printStackTrace();
            return false;
            /*PaymentFailedEvent failedEvent = new PaymentFailedEvent(event.getSlotId(), event.getUserId()); //bunu kontrol et 
            rabbitTemplate.convertAndSend("PaymentFailedEvent", failedEvent);*/
        }
    }

    private HttpHeaders createHeaders() {
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);

        String credentials = apiKey + ":" + secretKey;
        String base64Creds = Base64.getEncoder().encodeToString(credentials.getBytes());
        headers.add("Authorization", "Basic " + base64Creds);
        return headers;
    }
}
