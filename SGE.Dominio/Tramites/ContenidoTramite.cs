using SGE.Dominio.Comun; //en comun guardariamos el exception

namespace SGE.Dominio.Tramites;

public class ContenidoTramite
{
    public string Valor {get;}
    public ContenidoTramite(string valor_)
    {
        if (string.IsNullOrWhiteSpace(valor_))
        {
            throw new Dominioexception("El contenido del tramite no puede estar vacio");
        }
        Valor=valor_;
    }
}