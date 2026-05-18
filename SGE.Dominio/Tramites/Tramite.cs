using System.Dynamic;
using SGE.Dominio.Comun;
namespace SGE.Dominio.Tramites;
public class Tramite
{
    public Guid id{get;private set;}
    public Guid ExpedienteId{get;private set;}
    public EtiquetaTramite Etiqueta{get;private set;}
    public ContenidoTramite Contenido{get;private set;}
    public DateTime FechaCreacion{get;private set;}
    public DateTime FechaUltimaModificacion{get;private set;}
    public Guid UsuarioUltimoCambio{get;private set;}



    public Tramite(Guid expedienteId_, EtiquetaTramite etiqueta_, ContenidoTramite contenido_,Guid usuarioUltimoCambio_)
    {
        id=Guid.NewGuid();
        ExpedienteId=expedienteId_;
        Etiqueta=etiqueta_;
        Contenido=contenido_;
        FechaCreacion=DateTime.Now;
        FechaUltimaModificacion=FechaCreacion;
        UsuarioUltimoCambio=usuarioUltimoCambio_;

        Invariantes();
    }
    public void ModificarContenido(ContenidoTramite nuevoContenido, Guid idUsuario)
    {
        if (nuevoContenido == null)
        {
            throw new DominioException("El contenido a modificar no puede ser nulo.");
        }

        Contenido = nuevoContenido;
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
    }

    private void Invariantes()
    {
        // Acá se asume que Guid vacío cuenta como inválido, al igual que los nulos
        if (id == Guid.Empty || ExpedienteId == Guid.Empty || UsuarioUltimoCambio == Guid.Empty)
        {
            throw new DominioException("Los identificadores del trámite no pueden estar vacíos.");
        }

        if (Contenido == null)
        {
             throw new DominioException("El contenido del trámite es obligatorio.");
        }

        if (FechaUltimaModificacion < FechaCreacion)
        {
            throw new DominioException("La fecha de modificación no puede ser menor a la de creación.");
        }
    }
    public static Tramite Reconstruir(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, DateTime fechaCreacion, DateTime fechaModificacion, Guid usuario)
    {
        return new Tramite(id, expedienteId, etiqueta, contenido, fechaCreacion, fechaModificacion, usuario);
    }

    private Tramite(Guid _id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, DateTime fechaCreacion, DateTime fechaModificacion, Guid usuario)
    {
        id = _id;
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaModificacion;
        UsuarioUltimoCambio = usuario;

        Invariantes();
    }
}