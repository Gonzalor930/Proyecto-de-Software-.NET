namespace SGE.Dominio.Comun;

// Heredamos de la clase base Exception de C#
public class DominioException : Exception
{
    // Constructor con msj por defecto
    public DominioException() : base("Se produjo un error de validación en las reglas del dominio.")
    {
        
    }

    // Este es el constructor que usamos en las clases.
    // Recibe el mensaje y se lo pasa a la clase base.
    // Asi funciona por ejemplo en SGE.Dominio.Expedientes.Expediente;
    public DominioException(string mensaje) : base(mensaje)
    {
        
    }
}