package br.producer;

import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import jakarta.jms.ConnectionFactory;
import jakarta.jms.JMSContext;

@ApplicationScoped
public class JmsProducer {

    @Inject
    ConnectionFactory connectionFactory;

    public void send(String payload) {
        try (JMSContext context = connectionFactory.createContext()) {
            context.createProducer()
                   .send(context.createQueue("insert.message"), payload);
        }
    }
}
