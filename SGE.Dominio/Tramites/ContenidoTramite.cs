using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class ContenidoTramite
{
    public string Valor {get;}
    public ContenidoTramite(string valor_)
    {
        if (string.IsNullOrWhiteSpace(valor_))
        {
            throw new DominioException("El contenido del tramite no puede estar vacio");
        }
        Valor=valor_;
    }
    private ContenidoTramite(){}
}