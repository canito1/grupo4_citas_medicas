using CitasMedicas.API.Interfaces;
using CitasMedicas.API.Middleware;
using CitasMedicas.API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ─── Servicios ───────────────────────────────────────────────────────────────

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializar enums como strings (ej. "Programada" en vez de 0)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // Ignorar referencias circulares
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Registrar servicios como Singleton para mantener los datos en memoria durante la ejecución
builder.Services.AddSingleton<IPacienteService, PacienteService>();
builder.Services.AddSingleton<IMedicoService, MedicoService>();
builder.Services.AddSingleton<ICitaService, CitaService>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Citas Médicas API",
        Version = "v1",
        Description = "API RESTful para la gestión de citas médicas de una clínica privada. " +
                      "Permite registrar, consultar, actualizar y cancelar citas, pacientes y médicos.",
        Contact = new()
        {
            Name = "Equipo de Desarrollo Backend",
            Email = "dev@clinica.com"
        }
    });

    // Incluir comentarios XML en Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ─── Pipeline HTTP ────────────────────────────────────────────────────────────

// Middleware de manejo centralizado de excepciones (debe ir primero)
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Citas Médicas API v1");
        options.RoutePrefix = string.Empty; // Swagger en la raíz: http://localhost:PORT/
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
