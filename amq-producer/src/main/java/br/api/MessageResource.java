package br.api;

import br.producer.JmsProducer;
import jakarta.inject.Inject;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

@Path("/messages")
@Consumes(MediaType.TEXT_PLAIN)
public class MessageResource {

    @Inject
    JmsProducer producerService;

    @POST
    public Response sendMessage(String message) {
        producerService.send(message);
        return Response.ok("Mensagem enviada para a fila").build();
    }
}
