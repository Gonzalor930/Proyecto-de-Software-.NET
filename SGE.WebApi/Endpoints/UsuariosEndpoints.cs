using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.GestionUsuarios;
using SGE.Aplicacion.CasosDeUso;
using SGE.WebApi.Payloads;

namespace SGE.WebApi.Endpoints
{
    public static class UsuariosEndpoints
    {
        public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
        {
            var grupo = app.MapGroup("/api/usuarios").WithTags("Módulo de Usuarios");

            grupo.MapPost("/registro", (RegistroPayload payload, RegistrarUsuarioUseCase useCase) =>
            {
                var request = new RegistrarUsuarioRequest(payload.Nombre, payload.Correo, payload.ContrasenaPlana);
                var response = useCase.Ejecutar(request);

                return Results.Ok(new { Mensaje = "Usuario registrado", Id = response.UsuarioId });
            }); 

            grupo.MapPost("/login", (LoginPayload payload, LoginUseCase useCase) =>
            {
                var request = new LoginRequest(payload.Correo, payload.ContrasenaPlana);
                var response = useCase.Ejecutar(request);

                return Results.Ok(new { Token = response.Token });
            }); 

            grupo.MapPut("/mis-datos", (ModificarMisDatosPayload payload, ClaimsPrincipal user, ModificarMisDatosUseCase useCase) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                
                
                var request = new ModificarMisDatosRequest(userId, userId, payload.NuevoNombre, payload.NuevaContrasenaPlana);
                
                useCase.Ejecutar(request);
                return Results.NoContent(); 
            }).RequireAuthorization();

            grupo.MapGet("/", (ClaimsPrincipal user, ListarUsuariosUseCase useCase) =>
            {
                var idEjecutor = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                
                var usuarios = useCase.Ejecutar(idEjecutor); 
                
                return Results.Ok(usuarios);
            }).RequireAuthorization();

            grupo.MapDelete("/{idUsuarioAEliminar:guid}", (Guid idUsuarioAEliminar, ClaimsPrincipal user, EliminarUsuarioUseCase useCase) =>
            {
                var idEjecutor = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                useCase.Ejecutar(idEjecutor, idUsuarioAEliminar);
                
                return Results.NoContent();
            }).RequireAuthorization();

            grupo.MapPut("/{idUsuarioAModificar:guid}/permisos", (Guid idUsuarioAModificar, ModificarPermisosPayload payload, ClaimsPrincipal user, ModificarPermisosUsuarioUseCase useCase) =>
            {
                var idEjecutor = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                useCase.Ejecutar(idEjecutor, idUsuarioAModificar, payload.PermisosDeseados);
                
                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}