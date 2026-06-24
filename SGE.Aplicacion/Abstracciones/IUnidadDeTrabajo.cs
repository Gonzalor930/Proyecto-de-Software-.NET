public interface IUnidadDeTrabajo
{
    ///<summary>
    /// Confirma de forma atómica todos los cambios realizados en el contexto
    /// (Agregados, Modificados, Eliminados de los repositorios) en la base de datos real.
    /// </summary>
    void Guardar(); // Confirma de forma atómica los cambios en la base de datos
}
