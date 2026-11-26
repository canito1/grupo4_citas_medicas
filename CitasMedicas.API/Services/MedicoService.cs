using CitasMedicas.API.Exceptions;
using CitasMedicas.API.Interfaces;
using CitasMedicas.API.Models;

namespace CitasMedicas.API.Services;

/// <summary>
/// Servicio que gestiona la lógica de negocio para Médicos.
/// Los datos se almacenan en memoria usando una List<Medico>.
/// </summary>
public class MedicoService : IMedicoService
{
    private readonly List<Medico> _medicos = new();
    private int _nextId = 1;

    public MedicoService()
    {
        // Datos de ejemplo para facilitar pruebas con Postman
        _medicos.Add(new Medico
        {
            Id = _nextId++,
            Nombre = "Roberto",
            Apellido = "Mendoza",
            Especialidad = "Cardiología",
            NumeroColegiatura = "CMP-12345",
            Email = "r.mendoza@clinica.com"
        });
        _medicos.Add(new Medico
        {
            Id = _nextId++,
            Nombre = "Lucía",
            Apellido = "Torres",
            Especialidad = "Pediatría",
            NumeroColegiatura = "CMP-67890",
            Email = "l.torres@clinica.com"
        });
    }

    public IEnumerable<Medico> ObtenerTodos() => _medicos;

    public Medico ObtenerPorId(int id)
    {
        var medico = _medicos.FirstOrDefault(m => m.Id == id);
        if (medico is null)
            throw new NotFoundException("Médico", id);
        return medico;
    }

    public Medico Crear(Medico medico)
    {
        // Validar colegiatura única
        if (_medicos.Any(m => m.NumeroColegiatura == medico.NumeroColegiatura))
            throw new ConflictException($"Ya existe un médico con la colegiatura {medico.NumeroColegiatura}.");

        medico.Id = _nextId++;
        _medicos.Add(medico);
        return medico;
    }

    public Medico Actualizar(int id, Medico medico)
    {
        var existente = ObtenerPorId(id);

        // Validar colegiatura única excluyendo el médico actual
        if (_medicos.Any(m => m.NumeroColegiatura == medico.NumeroColegiatura && m.Id != id))
            throw new ConflictException($"Ya existe otro médico con la colegiatura {medico.NumeroColegiatura}.");

        existente.Nombre = medico.Nombre;
        existente.Apellido = medico.Apellido;
        existente.Especialidad = medico.Especialidad;
        existente.NumeroColegiatura = medico.NumeroColegiatura;
        existente.Email = medico.Email;

        return existente;
    }

    public void Eliminar(int id)
    {
        var existente = ObtenerPorId(id);
        _medicos.Remove(existente);
    }
}
