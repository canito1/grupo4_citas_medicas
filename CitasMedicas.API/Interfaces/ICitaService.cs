using CitasMedicas.API.Models;

namespace CitasMedicas.API.Interfaces;

/// <summary>
/// Contrato de operaciones CRUD y de negocio para la entidad Cita.
/// </summary>
public interface ICitaService
{
    IEnumerable<Cita> ObtenerTodas();
    IEnumerable<Cita> ObtenerPorPaciente(int pacienteId);
    IEnumerable<Cita> ObtenerPorMedico(int medicoId);
    Cita ObtenerPorId(int id);
    Cita Crear(Cita cita);
    Cita Actualizar(int id, Cita cita);
    Cita CambiarEstado(int id, EstadoCita nuevoEstado);
    void Eliminar(int id);
}
