using SGE.Dominio.Tramites;

namespace SGE.WebApi.Payloads
{
public record CrearTramitePayload(Guid ExpedienteId, string Contenido, int Etiqueta);
public record ModificarTramitePayload(string NuevoContenido);
}


