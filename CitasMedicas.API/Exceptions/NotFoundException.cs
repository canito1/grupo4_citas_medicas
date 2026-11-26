namespace CitasMedicas.API.Exceptions;

/// <summary>
/// Excepción lanzada cuando un recurso solicitado no es encontrado.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string recurso, int id)
        : base($"{recurso} con ID {id} no fue encontrado.")
    {
    }

    public NotFoundException(string mensaje)
        : base(mensaje)
    {
    }
}
