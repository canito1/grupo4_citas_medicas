using CitasMedicas.API.Models;

namespace CitasMedicas.API.Interfaces;

/// <summary>
/// Contrato de operaciones CRUD para la entidad Medico.
/// </summary>
public interface IMedicoService
{
    IEnumerable<Medico> ObtenerTodos();
    Medico ObtenerPorId(int id);
    Medico Crear(Medico medico);
    Medico Actualizar(int id, Medico medico);
    void Eliminar(int id);
}
