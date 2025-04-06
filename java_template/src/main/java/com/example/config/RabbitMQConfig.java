// RabbitMQConfig.java
package com.example.config;

import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
import org.springframework.amqp.support.converter.MessageConverter;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {
    public static final String PAYMENT_EXCHANGE = "payment.exchange";
    public static final String APPOINTMENT_EXCHANGE = "appointment.exchange";
    
    public static final String PAYMENT_INITIATED_QUEUE = "payment.initiated.queue";
    public static final String PAYMENT_SUCCESS_QUEUE = "payment.success.queue";
    public static final String PAYMENT_FAILED_QUEUE = "payment.failed.queue";
    public static final String REFUND_ISSUED_QUEUE = "payment.refund.queue";
    public static final String SLOT_CONFIRMED_QUEUE = "appointment.slot.confirmed.queue";
    public static final String APPOINTMENT_CANCELED_QUEUE = "appointment.canceled.queue";

    @Bean
    public DirectExchange paymentExchange() {
        return new DirectExchange(PAYMENT_EXCHANGE);
    }

    @Bean
    public DirectExchange appointmentExchange() {
        return new DirectExchange(APPOINTMENT_EXCHANGE);
    }

    @Bean
    public Queue paymentInitiatedQueue() {
        return new Queue(PAYMENT_INITIATED_QUEUE);
    }

    @Bean
    public Queue paymentSuccessQueue() {
        return new Queue(PAYMENT_SUCCESS_QUEUE);
    }

    @Bean
    public Queue paymentFailedQueue() {
        return new Queue(PAYMENT_FAILED_QUEUE);
    }

    @Bean
    public Queue refundIssuedQueue() {
        return new Queue(REFUND_ISSUED_QUEUE);
    }

    @Bean
    public Queue slotConfirmedQueue() {
        return new Queue(SLOT_CONFIRMED_QUEUE);
    }

    @Bean
    public Queue appointmentCanceledQueue() {
        return new Queue(APPOINTMENT_CANCELED_QUEUE);
    }

    @Bean
    public MessageConverter jsonMessageConverter() {
        return new Jackson2JsonMessageConverter();
    }

    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        RabbitTemplate template = new RabbitTemplate(connectionFactory);
        template.setMessageConverter(jsonMessageConverter());
        return template;
    }
}