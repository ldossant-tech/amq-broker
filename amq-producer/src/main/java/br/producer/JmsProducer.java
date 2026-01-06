package br.producer;

import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import jakarta.jms.JMSContext;
import jakarta.jms.Queue;

@ApplicationScoped
public class JmsProducer {

    @Inject
    JMSContext context;

    @Inject
    Queue messages; // nome vem do application.properties

    public void send(String payload) {
        context.createProducer().send(messages, payload);
    }
}
