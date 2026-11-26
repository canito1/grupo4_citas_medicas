using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.API.Models;

/// <summary>
/// Estado posible de una cita médica.
/// </summary>
public enum EstadoCita
{
    Programada,
    Confirmada,
    Cancelada,
    Completada
}

/// <summary>
/// Representa una cita médica agendada en la clínica.
/// </summary>
public class Cita
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El ID del paciente es obligatorio.")]
    public int PacienteId { get; set; }

    [Required(ErrorMessage = "El ID del médico es obligatorio.")]
    public int MedicoId { get; set; }

    [Required(ErrorMessage = "La fecha y hora de la cita son obligatorias.")]
    public DateTime FechaHora { get; set; }

    [Required(ErrorMessage = "El motivo de la cita es obligatorio.")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 500 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    public EstadoCita Estado { get; set; } = EstadoCita.Programada;

    [StringLength(1000, ErrorMessage = "Las observaciones no pueden superar los 1000 caracteres.")]
    public string? Observaciones { get; set; }

    // Propiedades de navegación (solo lectura, populadas por el servicio)
    public Paciente? Paciente { get; set; }
    public Medico? Medico { get; set; }
}
