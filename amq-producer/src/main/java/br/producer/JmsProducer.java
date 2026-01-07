package br.producer;

import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import jakarta.jms.ConnectionFactory;
import jakarta.jms.JMSContext;
import jakarta.jms.Queue;
import jakarta.annotation.Resource;

@ApplicationScoped
public class JmsProducer {

    @Inject
    ConnectionFactory connectionFactory;

    @Resource(lookup = "messages")
    Queue messages;

    public void send(String payload) {
        try (JMSContext context = connectionFactory.createContext()) {
            context.createProducer().send(messages, payload);
        }
    }
}
