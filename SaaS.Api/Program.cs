using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Habilita Controllers de API em C# e Cache em Memória de Altíssima Performance
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilita CORS para acesso universal
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Habilita a Interface Visual interativa do Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Harrison SaaS API v1");
    c.RoutePrefix = string.Empty; // Define o Swagger como página inicial visual (http://localhost:5205/)
});

// Middleware de Observabilidade e Coleta de Métricas (Prometheus)
app.UseRouting();
app.UseHttpMetrics(); // Mede automaticamente latência (ms), taxa de requisições por segundo e erros (500/404)

app.UseCors("AllowAll");
app.MapControllers();
app.MapMetrics();     // Expõe o endpoint /metrics para coleta do Prometheus e Grafana

app.Run();
