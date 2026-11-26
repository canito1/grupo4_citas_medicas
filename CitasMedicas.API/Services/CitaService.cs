using CitasMedicas.API.Exceptions;
using CitasMedicas.API.Interfaces;
using CitasMedicas.API.Models;

namespace CitasMedicas.API.Services;

/// <summary>
/// Servicio que gestiona la lógica de negocio para Citas médicas.
/// Los datos se almacenan en memoria usando una List<Cita>.
/// </summary>
public class CitaService : ICitaService
{
    private readonly List<Cita> _citas = new();
    private readonly IPacienteService _pacienteService;
    private readonly IMedicoService _medicoService;
    private int _nextId = 1;

    public CitaService(IPacienteService pacienteService, IMedicoService medicoService)
    {
        _pacienteService = pacienteService;
        _medicoService = medicoService;

        // Datos de ejemplo para facilitar pruebas con Postman
        _citas.Add(new Cita
        {
            Id = _nextId++,
            PacienteId = 1,
            MedicoId = 1,
            FechaHora = DateTime.Now.AddDays(2),
            Motivo = "Control cardiológico anual",
            Estado = EstadoCita.Programada
        });
    }

    public IEnumerable<Cita> ObtenerTodas()
    {
        return _citas.Select(PopularNavegacion);
    }

    public IEnumerable<Cita> ObtenerPorPaciente(int pacienteId)
    {
        // Validar que el paciente existe
        _pacienteService.ObtenerPorId(pacienteId);
        return _citas.Where(c => c.PacienteId == pacienteId).Select(PopularNavegacion);
    }

    public IEnumerable<Cita> ObtenerPorMedico(int medicoId)
    {
        // Validar que el médico existe
        _medicoService.ObtenerPorId(medicoId);
        return _citas.Where(c => c.MedicoId == medicoId).Select(PopularNavegacion);
    }

    public Cita ObtenerPorId(int id)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == id);
        if (cita is null)
            throw new NotFoundException("Cita", id);
        return PopularNavegacion(cita);
    }

    public Cita Crear(Cita cita)
    {
        // Validar que el paciente y el médico existen (lanza NotFoundException si no)
        _pacienteService.ObtenerPorId(cita.PacienteId);
        _medicoService.ObtenerPorId(cita.MedicoId);

        // Validar que la fecha sea futura
        if (cita.FechaHora <= DateTime.Now)
            throw new ArgumentException("La fecha y hora de la cita deben ser en el futuro.");

        // Validar que el médico no tenga otra cita a la misma hora
        bool horarioOcupado = _citas.Any(c =>
            c.MedicoId == cita.MedicoId &&
            c.Estado != EstadoCita.Cancelada &&
            Math.Abs((c.FechaHora - cita.FechaHora).TotalMinutes) < 30);

        if (horarioOcupado)
            throw new ConflictException("El médico ya tiene una cita en ese horario. Debe haber al menos 30 minutos de diferencia.");

        cita.Id = _nextId++;
        cita.Estado = EstadoCita.Programada;
        _citas.Add(cita);
        return PopularNavegacion(cita);
    }

    public Cita Actualizar(int id, Cita cita)
    {
        var existente = _citas.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException("Cita", id);

        // No se puede modificar una cita cancelada o completada
        if (existente.Estado == EstadoCita.Cancelada || existente.Estado == EstadoCita.Completada)
            throw new ConflictException($"No se puede modificar una cita con estado '{existente.Estado}'.");

        // Validar paciente y médico
        _pacienteService.ObtenerPorId(cita.PacienteId);
        _medicoService.ObtenerPorId(cita.MedicoId);

        // Validar fecha futura
        if (cita.FechaHora <= DateTime.Now)
            throw new ArgumentException("La fecha y hora de la cita deben ser en el futuro.");

        // Validar horario del médico excluyendo la cita actual
        bool horarioOcupado = _citas.Any(c =>
            c.Id != id &&
            c.MedicoId == cita.MedicoId &&
            c.Estado != EstadoCita.Cancelada &&
            Math.Abs((c.FechaHora - cita.FechaHora).TotalMinutes) < 30);

        if (horarioOcupado)
            throw new ConflictException("El médico ya tiene una cita en ese horario. Debe haber al menos 30 minutos de diferencia.");

        existente.PacienteId = cita.PacienteId;
        existente.MedicoId = cita.MedicoId;
        existente.FechaHora = cita.FechaHora;
        existente.Motivo = cita.Motivo;
        existente.Observaciones = cita.Observaciones;

        return PopularNavegacion(existente);
    }

    public Cita CambiarEstado(int id, EstadoCita nuevoEstado)
    {
        var existente = _citas.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException("Cita", id);

        // Reglas de transición de estado
        if (existente.Estado == EstadoCita.Cancelada)
            throw new ConflictException("No se puede cambiar el estado de una cita ya cancelada.");

        if (existente.Estado == EstadoCita.Completada)
            throw new ConflictException("No se puede cambiar el estado de una cita ya completada.");

        existente.Estado = nuevoEstado;
        return PopularNavegacion(existente);
    }

    public void Eliminar(int id)
    {
        var existente = _citas.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException("Cita", id);
        _citas.Remove(existente);
    }

    /// <summary>
    /// Popula las propiedades de navegación Paciente y Médico en una cita.
    /// Retorna una copia con los datos de navegación para no mutar el objeto almacenado.
    /// </summary>
    private Cita PopularNavegacion(Cita cita)
    {
        try
        {
            cita.Paciente = _pacienteService.ObtenerPorId(cita.PacienteId);
            cita.Medico = _medicoService.ObtenerPorId(cita.MedicoId);
        }
        catch (NotFoundException)
        {
            // Si el paciente o médico fue eliminado, se mantiene null
        }
        return cita;
    }
}
