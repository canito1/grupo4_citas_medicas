using CitasMedicas.API.Exceptions;
using CitasMedicas.API.Interfaces;
using CitasMedicas.API.Models;

namespace CitasMedicas.API.Services;

/// <summary>
/// Servicio que gestiona la lógica de negocio para Pacientes.
/// Los datos se almacenan en memoria usando una List<Paciente>.
/// </summary>
public class PacienteService : IPacienteService
{
    private readonly List<Paciente> _pacientes = new();
    private int _nextId = 1;

    public PacienteService()
    {
        // Datos de ejemplo para facilitar pruebas con Postman
        _pacientes.Add(new Paciente
        {
            Id = _nextId++,
            Nombre = "Ana",
            Apellido = "García",
            Dni = "12345678",
            Telefono = "987654321",
            Email = "ana.garcia@email.com",
            FechaNacimiento = new DateTime(1990, 5, 15)
        });
        _pacientes.Add(new Paciente
        {
            Id = _nextId++,
            Nombre = "Carlos",
            Apellido = "López",
            Dni = "87654321",
            Telefono = "912345678",
            Email = "carlos.lopez@email.com",
            FechaNacimiento = new DateTime(1985, 11, 22)
        });
    }

    public IEnumerable<Paciente> ObtenerTodos() => _pacientes;

    public Paciente ObtenerPorId(int id)
    {
        var paciente = _pacientes.FirstOrDefault(p => p.Id == id);
        if (paciente is null)
            throw new NotFoundException("Paciente", id);
        return paciente;
    }

    public Paciente Crear(Paciente paciente)
    {
        // Validar DNI único
        if (_pacientes.Any(p => p.Dni == paciente.Dni))
            throw new ConflictException($"Ya existe un paciente con el DNI {paciente.Dni}.");

        paciente.Id = _nextId++;
        _pacientes.Add(paciente);
        return paciente;
    }

    public Paciente Actualizar(int id, Paciente paciente)
    {
        var existente = ObtenerPorId(id);

        // Validar DNI único excluyendo el paciente actual
        if (_pacientes.Any(p => p.Dni == paciente.Dni && p.Id != id))
            throw new ConflictException($"Ya existe otro paciente con el DNI {paciente.Dni}.");

        existente.Nombre = paciente.Nombre;
        existente.Apellido = paciente.Apellido;
        existente.Dni = paciente.Dni;
        existente.Telefono = paciente.Telefono;
        existente.Email = paciente.Email;
        existente.FechaNacimiento = paciente.FechaNacimiento;

        return existente;
    }

    public void Eliminar(int id)
    {
        var existente = ObtenerPorId(id);
        _pacientes.Remove(existente);
    }
}
