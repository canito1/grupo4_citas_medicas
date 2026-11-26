using CitasMedicas.API.Interfaces;
using CitasMedicas.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

/// <summary>
/// Controlador para la gestión de Pacientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    /// <summary>
    /// Obtiene todos los pacientes registrados.
    /// </summary>
    /// <returns>Lista de pacientes.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Paciente>), StatusCodes.Status200OK)]
    public IActionResult ObtenerTodos()
    {
        var pacientes = _pacienteService.ObtenerTodos();
        return Ok(pacientes);
    }

    /// <summary>
    /// Obtiene un paciente por su ID.
    /// </summary>
    /// <param name="id">ID del paciente.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Paciente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        var paciente = _pacienteService.ObtenerPorId(id);
        return Ok(paciente);
    }

    /// <summary>
    /// Registra un nuevo paciente.
    /// </summary>
    /// <param name="paciente">Datos del paciente a registrar.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Paciente), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Crear([FromBody] Paciente paciente)
    {
        var creado = _pacienteService.Crear(paciente);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>
    /// Actualiza los datos de un paciente existente.
    /// </summary>
    /// <param name="id">ID del paciente a actualizar.</param>
    /// <param name="paciente">Nuevos datos del paciente.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Paciente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Actualizar(int id, [FromBody] Paciente paciente)
    {
        var actualizado = _pacienteService.Actualizar(id, paciente);
        return Ok(actualizado);
    }

    /// <summary>
    /// Elimina un paciente por su ID.
    /// </summary>
    /// <param name="id">ID del paciente a eliminar.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        _pacienteService.Eliminar(id);
        return NoContent();
    }
}
