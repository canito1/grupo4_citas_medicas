# Citas Médicas API

API RESTful desarrollada con **ASP.NET Core 8** para la gestión de citas médicas de una clínica privada. Permite registrar, consultar, actualizar y cancelar citas, pacientes y médicos. Los datos se almacenan en memoria durante la ejecución (sin base de datos).

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

Verificar instalación:
```bash
dotnet --version
# Debe mostrar 8.x.x
```

---

## Instalación y ejecución

```bash
# 1. Clonar o descargar el repositorio
cd citas-medicas

# 2. Restaurar dependencias
dotnet restore CitasMedicas.API/CitasMedicas.API.csproj

# 3. Ejecutar la API
dotnet run --project CitasMedicas.API/CitasMedicas.API.csproj
```

La API estará disponible en:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`
- **Swagger UI:** `http://localhost:5000` (raíz)

---

## Estructura del proyecto

```
CitasMedicas.API/
├── Controllers/          # Controladores REST (Pacientes, Médicos, Citas)
├── Exceptions/           # Excepciones personalizadas (NotFoundException, ConflictException)
├── Interfaces/           # Contratos de servicios (IPacienteService, IMedicoService, ICitaService)
├── Middleware/           # Middleware de manejo centralizado de excepciones
├── Models/               # Modelos de datos (Paciente, Medico, Cita, EstadoCita)
├── Services/             # Lógica de negocio con almacenamiento en memoria
├── Program.cs            # Configuración de la aplicación y DI
└── appsettings.json      # Configuración general
```

---

## Endpoints disponibles

### Pacientes — `/api/pacientes`

| Método | Ruta                   | Descripción                       |
|--------|------------------------|-----------------------------------|
| GET    | `/api/pacientes`       | Obtener todos los pacientes       |
| GET    | `/api/pacientes/{id}`  | Obtener un paciente por ID        |
| POST   | `/api/pacientes`       | Registrar un nuevo paciente       |
| PUT    | `/api/pacientes/{id}`  | Actualizar un paciente existente  |
| DELETE | `/api/pacientes/{id}`  | Eliminar un paciente              |

### Médicos — `/api/medicos`

| Método | Ruta                 | Descripción                     |
|--------|----------------------|---------------------------------|
| GET    | `/api/medicos`       | Obtener todos los médicos       |
| GET    | `/api/medicos/{id}`  | Obtener un médico por ID        |
| POST   | `/api/medicos`       | Registrar un nuevo médico       |
| PUT    | `/api/medicos/{id}`  | Actualizar un médico existente  |
| DELETE | `/api/medicos/{id}`  | Eliminar un médico              |

### Citas — `/api/citas`

| Método | Ruta                              | Descripción                              |
|--------|-----------------------------------|------------------------------------------|
| GET    | `/api/citas`                      | Obtener todas las citas                  |
| GET    | `/api/citas/{id}`                 | Obtener una cita por ID                  |
| GET    | `/api/citas/paciente/{pacienteId}`| Obtener citas de un paciente             |
| GET    | `/api/citas/medico/{medicoId}`    | Obtener citas de un médico               |
| POST   | `/api/citas`                      | Agendar una nueva cita                   |
| PUT    | `/api/citas/{id}`                 | Actualizar una cita existente            |
| PATCH  | `/api/citas/{id}/estado`          | Cambiar el estado de una cita            |
| DELETE | `/api/citas/{id}`                 | Eliminar una cita                        |

---

## Ejemplos de peticiones (Postman)

La colección [CitasMedicas.postman_collection.json](./CitasMedicas.postman_collection.json) incluye pruebas REST y validaciones para pacientes, médicos y citas. Impórtala desde **Postman → Import** y ejecuta la colección completa con **Run** para respetar el orden de las solicitudes y compartir los IDs creados entre pruebas. La variable `baseUrl` usa `http://localhost:5000` por defecto; cámbiala si la API se ejecuta en otra dirección o puerto.

### Crear paciente
```
POST /api/pacientes
Content-Type: application/json

{
  "nombre": "María",
  "apellido": "Ramos",
  "dni": "45678901",
  "telefono": "999888777",
  "email": "maria.ramos@email.com",
  "fechaNacimiento": "1995-03-10T00:00:00"
}
```

### Crear médico
```
POST /api/medicos
Content-Type: application/json

{
  "nombre": "Jorge",
  "apellido": "Vega",
  "especialidad": "Medicina General",
  "numeroColegiatura": "CMP-11111",
  "email": "j.vega@clinica.com"
}
```

### Agendar cita
```
POST /api/citas
Content-Type: application/json

{
  "pacienteId": 1,
  "medicoId": 1,
  "fechaHora": "2027-01-15T10:00:00",
  "motivo": "Consulta de rutina"
}
```

### Cambiar estado de una cita
```
PATCH /api/citas/1/estado
Content-Type: application/json

"Confirmada"
```

---

## Estados de una cita

| Estado      | Descripción                         |
|-------------|-------------------------------------|
| Programada  | Estado inicial al crear la cita     |
| Confirmada  | Cita confirmada por el paciente     |
| Cancelada   | Cita cancelada (estado final)       |
| Completada  | Cita realizada (estado final)       |

---

## Manejo de errores

Todos los errores retornan una respuesta JSON estandarizada:

```json
{
  "status": 404,
  "error": "NotFound",
  "mensaje": "Paciente con ID 99 no fue encontrado."
}
```

| Código | Situación                                      |
|--------|------------------------------------------------|
| 400    | Validación fallida o argumento inválido        |
| 404    | Recurso no encontrado                          |
| 409    | Conflicto (DNI duplicado, horario ocupado, etc.)|
| 500    | Error interno del servidor                     |
