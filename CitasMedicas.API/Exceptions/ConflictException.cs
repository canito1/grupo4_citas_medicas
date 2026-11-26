namespace CitasMedicas.API.Exceptions;

/// <summary>
/// Excepción lanzada cuando existe un conflicto con el estado actual del recurso
/// (ej. horario ya ocupado, DNI duplicado).
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string mensaje)
        : base(mensaje)
    {
    }
}
