using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using SGE.Aplicacion.Tramites;
using SGE.WebApi.Payloads;

namespace SGE.WebApi.Endpoints
{
        public static class TramitesEndpoints
    {
        public static void MapTramitesEndpoints(this IEndpointRouteBuilder app)
        {
            var grupo = app.MapGroup("/api/tramites").WithTags("Módulo de Trámites");

            // GET: Listar Tramites por Expediente
            grupo.MapGet("/expediente/{idExpediente:guid}", (Guid idExpediente, ListarTramitesPorExpedienteUseCase useCase) =>
            {
                var request = new ListarTramitesPorExpedienteRequest(idExpediente);
                return Results.Ok();
            }).RequireAuthorization();

            // POST: Alta de Tramite
            grupo.MapPost("/", (CrearTramitePayload payload, ClaimsPrincipal user, AgregarTramiteUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                
                var request = new AgregarTramiteRequest(userId, payload.ExpedienteId, payload.Contenido, payload.Etiqueta);
                return Results.Ok(); 
            }).RequireAuthorization();

            // PUT: Modificar Tramite
            grupo.MapPut("/{id:guid}", (Guid id, ModificarTramitePayload payload, ClaimsPrincipal user, ModificarTramiteUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                
                var request = new ModificarTramiteRequest(userId, id, payload.NuevoContenido);
                return Results.NoContent();
            }).RequireAuthorization();

            // DELETE: Baja de Trámite
            grupo.MapDelete("/{id:guid}", (Guid id, ClaimsPrincipal user, BajaTramiteUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                
                var request = new BajaTramiteRequest(userId, id);
                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}