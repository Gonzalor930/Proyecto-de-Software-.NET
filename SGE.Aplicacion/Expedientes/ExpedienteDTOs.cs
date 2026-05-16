namespace SGE.Aplicacion.Expedientes{
    public record AgregarExpedienteRequest(Guid IdUsuario, string DetalleCaratula);
    public record BajaExpedienteRequest(Guid IdUsuario, Guid ExpedienteId);
    public record ModificarCaratulaExpedienteRequest(Guid IdUsuario, Guid ExpedienteId, string NuevaCaratula);
    public record CambiarEstadoExpedienteRequest(Guid IdUsuario, Guid ExpedienteId, int NuevoEstado);
    public record AgregarExpedienteResponse(Guid ExpedienteId);
    public record BajaExpedienteResponse(bool Exito);
    public record ModificarCaratulaExpedienteResponse(bool Exito);
    public record CambiarEstadoExpedienteResponse(bool Exito);
    public record ListarExpedientesRequest();
    
    public record ExpedienteDTO(
        Guid Id, 
        string Caratula, 
        DateTime FechaCreacion, 
        DateTime FechaUltimaModificacion, 
        string Estado
    );
    
    public record ListarExpedientesResponse(IEnumerable<ExpedienteDTO> Expedientes);
}