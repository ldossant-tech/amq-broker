package br.producer;

import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import jakarta.jms.JMSContext;
import jakarta.jms.Queue;
import jakarta.annotation.Resource;

@ApplicationScoped
public class JmsProducer {

    @Inject
    JMSContext context;

    @Resource(lookup = "messages")
    Queue messages;

    public void send(String payload) {
        context.createProducer().send(messages, payload);
    }
}
