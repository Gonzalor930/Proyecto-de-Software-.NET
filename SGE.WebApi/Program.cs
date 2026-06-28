using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using SGE.Aplicacion;
using SGE.Infraestructura.Extensiones;
using SGE.WebApi.Middlewares;
using SGE.WebApi.Endpoints;
using SGE.Infraestructura.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// ==============================================================================
// 1. REGISTRO DE SERVICIOS (Contenedor de Inyección de Dependencias)
// ==============================================================================

// A. Configuración del Manejador Global de Excepciones 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// B. Configuración de Autenticación JWT estricta
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

//Documentación de la API (OpenAPI / Scalar)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); 

//Registro de Capas utilizando los archivos de Extension
// Asegurate de que los metodos en AplicacionExtensions.cs e InfraestructuraExtensions.cs se llamen asi
builder.Services.AddAplicacion();
builder.Services.AddInfraestructura(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SgeContext>();
    context.Database.EnsureCreated(); 
    context.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;");
}

// ==============================================================================
// 2. PIPELINE DE MIDDLEWARES (Orden estricto obligatorio)
// ==============================================================================

//Capturar excepciones DominioException
app.UseExceptionHandler(); 

//Validar quién es el usuario (Validación del Token JWT)
app.UseAuthentication();   

// 3º: Validar qué puede hacer el usuario (Validación de Permisos)
app.UseAuthorization();    

// ==============================================================================
// 3. CONFIGURACIÓN DE SCALAR (Interfaz Gráfica)
// ==============================================================================
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
if (app.Environment.IsDevelopment())
{
    // Expone el archivo JSON con la definición de la API
    app.MapOpenApi();
    
    // Configura y levanta la interfaz gráfica de Scalar
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Sistema de Gestión de Expedientes");
        options.WithTheme(ScalarTheme.Mars);
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecuritySchemes = new[] {"Bearer"}
        };
    });
}

// ==============================================================================
// 4. MAPEO DE ENDPOINTS (Utilizando tus 3 archivos de la carpeta Endpoints)
// ==============================================================================

app.MapUsuariosEndpoints();
app.MapExpedientesEndpoints();
app.MapTramitesEndpoints();

app.Run();