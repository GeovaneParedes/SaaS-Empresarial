var builder = WebApplication.CreateBuilder(args);

// Habilita Controllers de API em C#
builder.Services.AddControllers();
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

app.UseCors("AllowAll");
app.MapControllers();

app.Run();
