using CitasMedicas.API.Interfaces;
using CitasMedicas.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

/// <summary>
/// Controlador para la gestión de Médicos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MedicosController : ControllerBase
{
    private readonly IMedicoService _medicoService;

    public MedicosController(IMedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    /// <summary>
    /// Obtiene todos los médicos registrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Medico>), StatusCodes.Status200OK)]
    public IActionResult ObtenerTodos()
    {
        var medicos = _medicoService.ObtenerTodos();
        return Ok(medicos);
    }

    /// <summary>
    /// Obtiene un médico por su ID.
    /// </summary>
    /// <param name="id">ID del médico.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Medico), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        var medico = _medicoService.ObtenerPorId(id);
        return Ok(medico);
    }

    /// <summary>
    /// Registra un nuevo médico.
    /// </summary>
    /// <param name="medico">Datos del médico a registrar.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Medico), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Crear([FromBody] Medico medico)
    {
        var creado = _medicoService.Crear(medico);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>
    /// Actualiza los datos de un médico existente.
    /// </summary>
    /// <param name="id">ID del médico a actualizar.</param>
    /// <param name="medico">Nuevos datos del médico.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Medico), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Actualizar(int id, [FromBody] Medico medico)
    {
        var actualizado = _medicoService.Actualizar(id, medico);
        return Ok(actualizado);
    }

    /// <summary>
    /// Elimina un médico por su ID.
    /// </summary>
    /// <param name="id">ID del médico a eliminar.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        _medicoService.Eliminar(id);
        return NoContent();
    }
}
