using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;

namespace SGE.WebApi.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Inicializamos el objeto ProblemDetails requerido por el TP
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            // Mapeo de excepciones a codigos HTTP 
            switch (exception)
            {
                case DominioException dominioEx:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Error de regla de negocio";
                    problemDetails.Detail = dominioEx.Message;
                    break;

                case AutorizacionException authEx:
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Title = "Acceso denegado";
                    problemDetails.Detail = authEx.Message;
                    break;

                case EntidadNoEncontradaException notFoundEx:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Entidad no encontrada";
                    problemDetails.Detail = notFoundEx.Message;
                    break;

                default:
                    // Manejo de excepciones generico
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Error interno del servidor";
                    problemDetails.Detail = "Ocurrió un error inesperado al procesar la solicitud.";
                    break;
            }

            // Asigno el estado a la respuesta HTTP
            httpContext.Response.StatusCode = problemDetails.Status.Value;

            // pasamos la respuesta a formato JSON
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}