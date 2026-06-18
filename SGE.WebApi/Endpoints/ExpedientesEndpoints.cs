using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace SGE.WebApi.Endpoints;

public static class ExpedientesEndpoints
{
    // El this IEndpointRouteBuilder es lo que nos permite usar este método en el Program.cs
    public static void MapExpedientesEndpoints(this IEndpointRouteBuilder app)
    {
        //Agrupamos todas las rutas para no repetir "/api/expedientes" en cada una
        var group = app.MapGroup("/api/expedientes");

        //Aca van a ir los endpoints
        
    }
}