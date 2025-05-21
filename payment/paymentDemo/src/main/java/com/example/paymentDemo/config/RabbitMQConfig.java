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
    public static final String EXCHANGE = "payment.exchange";
    public static final String INITIATED_QUEUE = "payment.initiated.queue";
    public static final String SUCCEEDED_QUEUE = "payment.succeeded.queue";
    public static final String FAILED_QUEUE = "payment.failed.queue";
    public static final String REFUNDED_QUEUE = "payment.refunded.queue";
    public static final String PROCESS_QUEUE = "payment.process.queue";

    @Bean
    public TopicExchange paymentExchange() {
        return new TopicExchange(EXCHANGE);
    }

    @Bean
    public Jackson2JsonMessageConverter jsonMessageConverter() {
        ObjectMapper objectMapper = new ObjectMapper();
        objectMapper.setPropertyNamingStrategy(PropertyNamingStrategies.UPPER_CAMEL_CASE); // ihtiyaca göre
        return new Jackson2JsonMessageConverter(objectMapper);
    }
    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        RabbitTemplate rabbitTemplate = new RabbitTemplate(connectionFactory);
        rabbitTemplate.setMessageConverter(new Jackson2JsonMessageConverter()); //jsonMessageConverter?
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
    public TopicExchange userExchange() {
    return new TopicExchange("user.exchange");
    }   

    @Bean
    public Queue getSavedCardQueue() {
        return new Queue("getSavedCardQueue");
    }

    @Bean
    public Binding reservationQueueBinding(Queue reservationQueue, @Qualifier("paymentExchange") TopicExchange paymentExchange) {
        return BindingBuilder.bind(reservationQueue).to(paymentExchange).with("reservation.created");
    }
    @Bean
    public Binding getSavedCardBinding(@Qualifier("getSavedCardQueue") Queue getSavedCardQueue,
                                   @Qualifier("userExchange") TopicExchange userExchange) {
    return BindingBuilder.bind(getSavedCardQueue).to(userExchange).with("user.getSavedCard");
    }
    @Bean
    public Binding paymentSucceededBinding(Queue PaymentSucceededQueue, @Qualifier("paymentExchange") TopicExchange paymentExchange) {
        return BindingBuilder.bind(PaymentSucceededQueue).to(paymentExchange).with("payment.succeeded");
    }
    @Bean
    public Binding paymentFailedBinding(Queue paymentFailedQueue, @Qualifier("paymentExchange") TopicExchange paymentExchange) {
        return BindingBuilder.bind(paymentFailedQueue).to(paymentExchange).with("payment.failed");
    }


    /*
    @Bean
    public Binding reservationQueueBinding(Queue reservationQueue, TopicExchange exchange) {
         return BindingBuilder.bind(reservationQueue).to(exchange).with("reservation.created");
     }
    @Bean
    public Binding paymentSucceededBinding(Queue paymentSucceededQueue, TopicExchange exchange) {
    return BindingBuilder.bind(paymentSucceededQueue).to(exchange).with("payment.succeeded");
    }
    @Bean
    public Binding paymentFailedBinding(Queue paymentFailedQueue, TopicExchange exchange) {
    return BindingBuilder.bind(paymentFailedQueue).to(exchange).with("payment.failed");
    }*/
}