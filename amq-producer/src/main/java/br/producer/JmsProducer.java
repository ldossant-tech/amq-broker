package br.producer;

import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import jakarta.jms.ConnectionFactory;
import jakarta.jms.JMSContext;

import org.eclipse.microprofile.config.inject.ConfigProperty;
import org.jboss.logging.Logger;

@ApplicationScoped
public class JmsProducer {

    private static final Logger LOG = Logger.getLogger(JmsProducer.class);

    @Inject
    ConnectionFactory connectionFactory;

    @ConfigProperty(name = "amq.queue.name")
    String queueName;

    public void send(String payload) {
        LOG.debugf("Iniciando envio de mensagem para a fila [%s]", queueName);

        try (JMSContext context = connectionFactory.createContext()) {

            context.createProducer()
                   .send(context.createQueue(queueName), payload);

            LOG.infof(
                "Mensagem enviada com sucesso para a fila [%s]. Payload size=%d",
                queueName,
                payload != null ? payload.length() : 0
            );

        } catch (Exception e) {
            LOG.errorf(
                e,
                "Erro ao enviar mensagem para a fila [%s]. Payload=%s",
                queueName,
                payload
            );
            throw e; // deixa o Resource decidir como responder
        }
    }
}
