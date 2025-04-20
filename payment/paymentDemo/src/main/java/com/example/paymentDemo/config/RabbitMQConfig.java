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

    @Bean
    public Queue reservationQueue() {
        return new Queue("reservationQueue", false); // durable olsun
    }
    @Bean
    public Queue PaymentSucceededQueue() {
        return new Queue("PaymentSucceededEvent", true);
    }

     @Bean
     public Binding reservationQueueBinding(Queue reservationQueue, TopicExchange exchange) {
         return BindingBuilder.bind(reservationQueue).to(exchange).with("reservation.created");
     }
    //  @Bean
    //  public Binding paymentSucceededBinding(Queue paymentSucceededQueue, TopicExchange topicExchange) {
    //      return BindingBuilder.bind(paymentSucceededQueue).to(topicExchange).with("PaymentSucceeded");
    //  }
}
