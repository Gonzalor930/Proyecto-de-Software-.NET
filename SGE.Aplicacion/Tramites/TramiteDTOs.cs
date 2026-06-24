using SGE.Dominio.Tramites;


namespace SGE.Aplicacion.Tramites{
    public record AgregarTramiteRequest(Guid IdUsuario, Guid ExpedienteId, string Contenido, int Etiqueta);
    public record BajaTramiteRequest(Guid IdUsuario, Guid TramiteId);
    public record ModificarTramiteRequest(Guid IdUsuario, Guid TramiteId, string NuevoContenido);
    public record AgregarTramiteResponse(Guid TramiteId);
    public record BajaTramiteResponse(bool Exito);
    public record ModificarTramiteResponse(bool Exito);
    public record TramiteDTO
    (
        Guid id,
        Guid ExpedienteId,
        string Etiqueta,
        string Contenido,
        DateTime FechaCreacion,
        DateTime FechaUltimaModificacion,
        Guid UsuarioUltimoCambio
    );
    public record ListarTramitesPorExpedienteRequest(Guid ExpedienteId);
    public record ListarTramitesPorExpedienteResponse(IEnumerable<TramiteDTO> Tramites);
}