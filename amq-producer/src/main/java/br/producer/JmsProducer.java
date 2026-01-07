package br.producer;

import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import jakarta.jms.JMSContext;
import jakarta.jms.Queue;
import org.eclipse.microprofile.config.inject.ConfigProperty;

@ApplicationScoped
public class JmsProducer {

    @Inject
    JMSContext context;

    @ConfigProperty(name = "amq.queue.name")
    String queueName;

    public void send(String payload) {
        Queue queue = context.createQueue(queueName);
        context.createProducer().send(queue, payload);
    }
}
