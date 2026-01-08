package br.api;

import br.producer.JmsProducer;
import jakarta.inject.Inject;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

import org.jboss.logging.Logger;

@Path("/messages")
@Consumes(MediaType.TEXT_PLAIN)
@Produces(MediaType.TEXT_PLAIN)
public class MessageResource {

    private static final Logger LOG = Logger.getLogger(MessageResource.class);

    @Inject
    JmsProducer producerService;

    @POST
    public Response sendMessage(String message) {

        LOG.debug("Recebida requisição POST /messages");

        // Validação básica
        if (message == null || message.isBlank()) {
            LOG.warn("Payload inválido recebido (null ou vazio)");
            return Response
                    .status(Response.Status.BAD_REQUEST)
                    .entity("Mensagem não pode ser vazia")
                    .build();
        }

        try {
            producerService.send(message);

            LOG.info("Mensagem processada e enviada para o broker com sucesso");

            return Response
                    .ok("Mensagem enviada para a fila")
                    .build();

        } catch (Exception e) {
            LOG.error("Falha ao enviar mensagem para o broker", e);

            return Response
                    .status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity("Erro ao enviar mensagem para o broker")
                    .build();
        }
    }
}
