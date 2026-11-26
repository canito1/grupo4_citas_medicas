using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.API.Models;

/// <summary>
/// Representa un paciente registrado en la clínica.
/// </summary>
public class Paciente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener solo dígitos.")]
    public string Dni { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }
}
