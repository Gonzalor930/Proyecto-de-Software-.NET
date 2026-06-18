using SGE.Aplicacion;
using SGE.Infraestructura;
using SGE.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. REGISTRO DE DEPENDENCIAS (Inyección de las distintas capas)
// ============================================================

builder.Services.AddAplicacion();

// Como ya tenés el string adentro del método, lo llamamos directamente
builder.Services.AddInfraestructura(); 


var app = builder.Build();

// ============================================================
// 2. CONFIGURACIÓN DEL PIPELINE Y MAPEO DE RUTAS
// ============================================================

// Llamamos a los métodos estáticos
app.MapExpedientesEndpoints();
app.MapUsuariosEndpoints();
app.MapTramitesEndpoints();

app.Run();