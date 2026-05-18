using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; }
    public Caratula Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio{ get; private set; }
    public EstadoExpediente Estado { get; private set; } 
    

    public Expediente(Caratula caratula, Guid usuarioUltimoCambio)
    {
        Id = Guid.NewGuid();
        Caratula = caratula ?? throw new DominioException("La caratula no puede ser nula");
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = DateTime.Now;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        Estado = EstadoExpediente.RecienIniciado;
    }   
    public void ModificarCaratula(Caratula nuevaCaratula, Guid idUsuario)
    {
        Caratula = nuevaCaratula ?? throw new DominioException("La caratula no puede ser nula");
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
    }
    public bool ActualizarEstado(EtiquetaTramite? ultimaEtiqueta, Guid idUsuario)
    {
        EstadoExpediente estadoAnterior = Estado;

        if(ultimaEtiqueta == null)
        {
            Estado = EstadoExpediente.RecienIniciado;
        }
        else
        {
            switch (ultimaEtiqueta)
            {
                case EtiquetaTramite.Resolucion:
                    Estado = EstadoExpediente.ConResolucion;
                    break;
                case EtiquetaTramite.PaseAEstudio:
                    Estado = EstadoExpediente.ParaResolver;
                    break;
                case EtiquetaTramite.PaseAlArchivo:
                    Estado = EstadoExpediente.Finalizado;
                    break;
            }
        }
        if(Estado == estadoAnterior)
        {
            return false;
        }
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
        return true;
    }
    public void CambiarEstado(EstadoExpediente nuevoEstado, Guid idUsuario)
    {
        Estado = nuevoEstado;
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
    }
    public static Expediente Reconstruir(Guid id, Caratula caratula, DateTime fechaCreacion, DateTime fechaModif, Guid usuarioId, EstadoExpediente estado)
    {
        return new Expediente(id, caratula, fechaCreacion, fechaModif, usuarioId, estado);
    }

    private Expediente(Guid id, Caratula caratula, DateTime fechaCreacion, DateTime fechaModif, Guid usuarioId, EstadoExpediente estado)
    {
        Id = id;
        Caratula = caratula;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaModif;
        UsuarioUltimoCambio = usuarioId;
        Estado = estado;
    }
}