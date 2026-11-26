using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.API.Models;

/// <summary>
/// Representa un médico registrado en la clínica.
/// </summary>
public class Medico
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "La especialidad debe tener entre 3 y 100 caracteres.")]
    public string Especialidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de colegiatura es obligatorio.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "El número de colegiatura debe tener entre 5 y 20 caracteres.")]
    public string NumeroColegiatura { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; } = string.Empty;
}
