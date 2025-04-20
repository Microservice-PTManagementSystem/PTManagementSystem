/*package com.example.paymentDemo.config;

import org.springframework.amqp.core.Binding;
import org.springframework.amqp.core.BindingBuilder;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.core.TopicExchange;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {

    public static final String PAYMENT_QUEUE = "slot.reserved.queue";
    public static final String PAYMENT_SUCCESS_QUEUE = "payment.success.queue";
    public static final String PAYMENT_FAILED_QUEUE = "payment.failed.queue";
    public static final String EXCHANGE = "pt.topic.exchange";

    @Bean
    public TopicExchange exchange() {
        return new TopicExchange(EXCHANGE);
    }

    @Bean
    public Queue paymentQueue() {
        return new Queue(PAYMENT_QUEUE);
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
    public Binding slotReservedBinding() {
        return BindingBuilder.bind(paymentQueue()).to(exchange()).with("slot.reserved");
    }

    @Bean
    public Binding paymentSuccessBinding() {
        return BindingBuilder.bind(paymentSuccessQueue()).to(exchange()).with("payment.success");
    }

    @Bean
    public Binding paymentFailedBinding() {
        return BindingBuilder.bind(paymentFailedQueue()).to(exchange()).with("payment.failed");
    }
}



*/
package com.example.paymentDemo.config;

import org.springframework.amqp.core.Binding;
import org.springframework.amqp.core.BindingBuilder;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.core.TopicExchange;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.PropertyNamingStrategies;

@Configuration
public class RabbitMQConfig {

    //public static final String RESERVATION_QUEUE = "reservationQueue";

    /*public static final String PAYMENT_QUEUE = "slot.reserved.queue";
    public static final String PAYMENT_SUCCESS_QUEUE = "payment.success.queue";
    public static final String PAYMENT_FAILED_QUEUE = "payment.failed.queue";*/
    public static final String EXCHANGE = "pt.topic.exchange";


    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        RabbitTemplate template = new RabbitTemplate(connectionFactory);
        template.setMessageConverter(jsonMessageConverter());
        return template;
    }

    @Bean
    public Jackson2JsonMessageConverter jsonMessageConverter() {
        ObjectMapper objectMapper = new ObjectMapper();
        objectMapper.setPropertyNamingStrategy(PropertyNamingStrategies.UPPER_CAMEL_CASE); // ihtiyaca göre
        return new Jackson2JsonMessageConverter(objectMapper);
    }

    
   
    @Bean
    public TopicExchange exchange() {
        return new TopicExchange(EXCHANGE);
    }   

    
   /* @Bean
    public Queue paymentQueue() {
        return new Queue("paymentQueue", true); // durable olsun
    }

    @Bean
    public Binding paymentQueueBinding(Queue paymentQueue, TopicExchange exchange) {
        return BindingBuilder.bind(paymentQueue).to(exchange).with("payment.#");
    } */

    @Bean
    public Queue reservationQueue() {
        return new Queue("reservationQueue", false); // durable olsun
    }

    @Bean
    public Binding reservationQueueBinding(Queue reservationQueue, TopicExchange exchange) {
        return BindingBuilder.bind(reservationQueue).to(exchange).with("reservation.created");
    }
    

    
}
