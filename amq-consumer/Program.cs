using Interfaces;
using Services;
using Services.Configuration;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Configuration
// =======================

// appsettings.json + appsettings.{Environment}.json
// + Variáveis de ambiente (OpenShift sobrescreve automaticamente)
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// =======================
// Services
// =======================

// Controllers (opcional – útil para health/status)
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔧 Bind correto da configuração do AMQ
builder.Services.Configure<AmqBrokerOptions>(
    builder.Configuration.GetSection("AmqBroker")
);

// AMQ Consumer (worker)
builder.Services.AddSingleton<IQueueConsumer, AmqConsumerService>();

// 🔥 ESSENCIAL: inicia o consumer junto com a aplicação
builder.Services.AddHostedService<AmqConsumerHostedService>();

var app = builder.Build();

// =======================
// Pipeline
// =======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
        options.RoutePrefix = "swagger"; // /swagger
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();