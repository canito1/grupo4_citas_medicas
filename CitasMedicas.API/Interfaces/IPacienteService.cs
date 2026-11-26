using CitasMedicas.API.Models;

namespace CitasMedicas.API.Interfaces;

/// <summary>
/// Contrato de operaciones CRUD para la entidad Paciente.
/// </summary>
public interface IPacienteService
{
    IEnumerable<Paciente> ObtenerTodos();
    Paciente ObtenerPorId(int id);
    Paciente Crear(Paciente paciente);
    Paciente Actualizar(int id, Paciente paciente);
    void Eliminar(int id);
}
