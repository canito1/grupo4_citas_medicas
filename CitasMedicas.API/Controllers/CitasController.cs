using CitasMedicas.API.Interfaces;
using CitasMedicas.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

/// <summary>
/// Controlador para la gestión de Citas médicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CitasController : ControllerBase
{
    private readonly ICitaService _citaService;

    public CitasController(ICitaService citaService)
    {
        _citaService = citaService;
    }

    /// <summary>
    /// Obtiene todas las citas médicas registradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Cita>), StatusCodes.Status200OK)]
    public IActionResult ObtenerTodas()
    {
        var citas = _citaService.ObtenerTodas();
        return Ok(citas);
    }

    /// <summary>
    /// Obtiene una cita por su ID.
    /// </summary>
    /// <param name="id">ID de la cita.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Cita), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        var cita = _citaService.ObtenerPorId(id);
        return Ok(cita);
    }

    /// <summary>
    /// Obtiene todas las citas de un paciente específico.
    /// </summary>
    /// <param name="pacienteId">ID del paciente.</param>
    [HttpGet("paciente/{pacienteId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Cita>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorPaciente(int pacienteId)
    {
        var citas = _citaService.ObtenerPorPaciente(pacienteId);
        return Ok(citas);
    }

    /// <summary>
    /// Obtiene todas las citas de un médico específico.
    /// </summary>
    /// <param name="medicoId">ID del médico.</param>
    [HttpGet("medico/{medicoId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Cita>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorMedico(int medicoId)
    {
        var citas = _citaService.ObtenerPorMedico(medicoId);
        return Ok(citas);
    }

    /// <summary>
    /// Agenda una nueva cita médica.
    /// </summary>
    /// <param name="cita">Datos de la cita a agendar.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Cita), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Crear([FromBody] Cita cita)
    {
        var creada = _citaService.Crear(cita);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    /// <summary>
    /// Actualiza los datos de una cita existente.
    /// </summary>
    /// <param name="id">ID de la cita a actualizar.</param>
    /// <param name="cita">Nuevos datos de la cita.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Cita), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Actualizar(int id, [FromBody] Cita cita)
    {
        var actualizada = _citaService.Actualizar(id, cita);
        return Ok(actualizada);
    }

    /// <summary>
    /// Cambia el estado de una cita (Programada, Confirmada, Cancelada, Completada).
    /// </summary>
    /// <param name="id">ID de la cita.</param>
    /// <param name="estado">Nuevo estado de la cita.</param>
    [HttpPatch("{id:int}/estado")]
    [ProducesResponseType(typeof(Cita), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult CambiarEstado(int id, [FromBody] EstadoCita estado)
    {
        var actualizada = _citaService.CambiarEstado(id, estado);
        return Ok(actualizada);
    }

    /// <summary>
    /// Elimina una cita por su ID.
    /// </summary>
    /// <param name="id">ID de la cita a eliminar.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        _citaService.Eliminar(id);
        return NoContent();
    }
}
