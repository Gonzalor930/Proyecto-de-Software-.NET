using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using SGE.Aplicacion.Expedientes;



namespace SGE.WebApi.Endpoints
{
    public static class ExpedientesEndpoints
    {
        public static void MapExpedientesEndpoints(this IEndpointRouteBuilder app)
        {
            var grupo = app.MapGroup("/api/expedientes").WithTags("Modulo de Expedientes");

            // GET: Listar todos
            grupo.MapGet("/", (ListarExpedientesUseCase useCase ) =>
            {
               // var request = new ListarExpedientesRequest(); la marca como no necesaria el IDE
                var response = useCase.Ejecutar();
                return Results.Ok(response);
            }).RequireAuthorization();

            // GET: Obtener por ID
            grupo.MapGet("/{id:guid}", (Guid id,ObtenerExpedienteUseCase useCase) =>
            {
                var request = new ObtenerExpedienteRequest(id);
                var response = useCase.Ejecutar(request);
                return Results.Ok(response);
            }).RequireAuthorization();

            // POST: Alta de Expediente
            grupo.MapPost("/", (CrearExpedientePayload payload, ClaimsPrincipal user, AltaExpedienteUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                
                // Ensambla el DTO en la capa de Aplicacion
                var request = new AgregarExpedienteRequest(userId, payload.DetalleCaratula);
                
                // var response = useCase.Ejecutar(request);
                return Results.Ok(); // Puede devolver el response.ExpedienteId aca
            }).RequireAuthorization();

            // PUT: Modificar Caratula
            grupo.MapPut("/{id:guid}/caratula", (Guid id, ModificarCaratulaPayload payload, ClaimsPrincipal user, ModificarCaratulaExpedienteUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var request = new ModificarCaratulaExpedienteRequest(userId, id, payload.NuevaCaratula);
                
                // var response = useCase.Ejecutar(request);
                return Results.NoContent();
            }).RequireAuthorization();

            // PUT: Cambio de Estado Manual
            grupo.MapPut("/{id:guid}/estado", (Guid id, CambiarEstadoPayload payload, ClaimsPrincipal user, CambiarEstadoExpedienteUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var request = new CambiarEstadoExpedienteRequest(userId, id, payload.NuevoEstado);
                
                //var response = useCase.Ejecutar(request);
                return Results.NoContent();
            }).RequireAuthorization();

            // DELETE: Baja de Expediente
            grupo.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, BajaExpedienteUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var request = new BajaExpedienteRequest(userId, id);
                
                // var response = useCase.Ejecutar(request);
                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}