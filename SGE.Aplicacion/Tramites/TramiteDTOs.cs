namespace SGE.Aplicacion.Tramites{
    public record AgregarTramiteRequest(Guid IdUsuario, Guid ExpedienteId, string Contenido, int Etiqueta);
    public record BajaTramiteRequest(Guid IdUsuario, Guid TramiteId);
    public record ModificarTramiteRequest(Guid IdUsuario, Guid TramiteId, string NuevoContenido);
    public record AgregarTramiteResponse(Guid TramiteId);
    public record BajaTramiteResponse(bool Exito);
    public record ModificarTramiteResponse(bool Exito);
}