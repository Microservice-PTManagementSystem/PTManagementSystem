package com.example.paymentDemo.config;

import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
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
  /* 
    @Bean
    public Queue initiatedQueue() {
        return new Queue(INITIATED_QUEUE,true);
    }

    @Bean
    public Queue succeededQueue() {
        return new Queue(SUCCEEDED_QUEUE,true);
    }

    @Bean
    public Queue failedQueue() {
        return new Queue(FAILED_QUEUE,true);
    }

    @Bean
    public Queue refundedQueue() {
        return new Queue(REFUNDED_QUEUE,true);
    }*/
    /* 
    @Bean
    public Binding succeededBinding() {
        return BindingBuilder.bind(succeededQueue())
            .to(paymentExchange())
            .with("payment.succeeded");
    }*/
    /* 
    @Bean
    public Binding failedBinding() {
        return BindingBuilder.bind(failedQueue())
            .to(paymentExchange())
            .with("payment.failed");
    }
    @Bean
    public Binding refundedBinding() {
        return BindingBuilder.bind(refundedQueue())
            .to(paymentExchange())
            .with("payment.refunded");
    }*/


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
/* 
    @Bean
    public Binding initiatedBinding() {
        return BindingBuilder.bind(initiatedQueue())
            .to(paymentExchange())
            .with("payment.initiated");
    }
  */

    @Bean
    public Queue reservationQueue() {
        return new Queue("reservationQueue", false); // durable olsun
    }

    @Bean
    public Queue PaymentSucceededQueue() {
        return new Queue("PaymentSucceededEvent", true);
    }

     @Bean
     public Binding reservationQueueBinding(Queue reservationQueue, TopicExchange paymentExchange) {
         return BindingBuilder.bind(reservationQueue).to(paymentExchange).with("reservation.created");
     }
    //  @Bean
    //  public Binding paymentSucceededBinding(Queue paymentSucceededQueue, TopicExchange topicExchange) {
    //      return BindingBuilder.bind(paymentSucceededQueue).to(topicExchange).with("PaymentSucceeded");
    //  }

    @Bean
    public Queue processQueue() {
        return new Queue(PROCESS_QUEUE);
    }

    @Bean
    public Binding processBinding(Queue processQueue, TopicExchange paymentExchange) {
        return BindingBuilder.bind(processQueue())
            .to(paymentExchange)
            .with("payment.process");
    }
}