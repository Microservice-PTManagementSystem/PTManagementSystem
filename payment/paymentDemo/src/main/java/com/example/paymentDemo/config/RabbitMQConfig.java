package com.example.paymentDemo.config;

import org.springframework.amqp.core.*;
import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.PropertyNamingStrategies;

@Configuration
public class RabbitMQConfig {

    @Bean
    @Qualifier("paymentExchange")
    public TopicExchange paymentExchange() {
        return new TopicExchange("payment.exchange");
    }
    @Bean
    @Qualifier("userExchange")
    public TopicExchange userExchange() {
    return new TopicExchange("user.exchange");
    }   

    @Bean
    @Qualifier("userPaymentExchange")
    public TopicExchange userPaymentExchange() {
    return new TopicExchange("user.payment.exchange");
    }

    @Bean
    public Jackson2JsonMessageConverter jsonMessageConverter() {
        ObjectMapper objectMapper = new ObjectMapper();
        objectMapper.setPropertyNamingStrategy(PropertyNamingStrategies.UPPER_CAMEL_CASE); // ihtiyaca göre
        return new Jackson2JsonMessageConverter(objectMapper);
    }
    /*@Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        RabbitTemplate rabbitTemplate = new RabbitTemplate(connectionFactory);
        rabbitTemplate.setMessageConverter(new Jackson2JsonMessageConverter()); //jsonMessageConverter?
        return rabbitTemplate;
    }*/
    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory, Jackson2JsonMessageConverter messageConverter) {
    RabbitTemplate rabbitTemplate = new RabbitTemplate(connectionFactory);
    rabbitTemplate.setMessageConverter(messageConverter);
    return rabbitTemplate;
    }


    @Bean
    public Queue reservationQueue() {
        return new Queue("reservationQueue", false); // durable olsun
    }


    @Bean
    public Queue PaymentSucceededQueue() {
        return new Queue("PaymentSucceededEvent", true);
    }
    @Bean
    public Queue paymentFailedQueue() {
    return new Queue("PaymentFailedEvent", true);
    }
    @Bean
    public Queue PaymentInfoRequested() {
        return new Queue("PaymentInfoRequested", true);
    }
    @Bean
    public Queue PaymentInfoSentEvent() {
    return new Queue("PaymentInfoSentEvent", true); // durable: true
    }

/*
    @Bean
    public Binding reservationQueueBinding(Queue reservationQueue, @Qualifier("paymentExchange") TopicExchange exchange) {
        return BindingBuilder.bind(reservationQueue).to(exchange).with("reservation.created");
    }*/
    /*
    @Bean
    public Binding PaymentInfoRequestedBinding(@Qualifier("PaymentInfoRequested") Queue PaymentInfoRequested,
                                   @Qualifier("userExchange") TopicExchange exchange) {
    return BindingBuilder.bind(PaymentInfoRequested).to(exchange).with("user.getSavedCard");
    }
    @Bean
    public Binding paymentInfoSentBinding(@Qualifier("PaymentInfoSentEvent") Queue PaymentInfoSentEvent,
                                     @Qualifier("userPaymentExchange") TopicExchange exchange) {
    return BindingBuilder.bind(PaymentInfoSentEvent).to(exchange).with("user.payment.exchange");
    }*/

    @Bean
    public Binding paymentSucceededBinding(Queue PaymentSucceededQueue, @Qualifier("paymentExchange") TopicExchange exchange) {
        return BindingBuilder.bind(PaymentSucceededQueue).to(exchange).with("payment.succeeded");
    }
    @Bean
    public Binding paymentFailedBinding(Queue paymentFailedQueue, @Qualifier("paymentExchange") TopicExchange exchange) {
        return BindingBuilder.bind(paymentFailedQueue).to(exchange).with("payment.failed");
    }

}