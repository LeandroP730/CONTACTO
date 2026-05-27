# Contactos API

API REST en **.NET 10** para gestión de contactos con almacenamiento en memoria.  
No requiere base de datos ni configuración adicional.

---

## Requisitos

| Herramienta | Versión mínima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/es-es/download/dotnet/10.0) | **10.0** |

Verificá tu versión instalada con:

```bash
dotnet --version
```

---

## Ejecución local

```bash
# 1. Clonar el repositorio
git clone <url-del-repositorio>
cd CONTACTO

# 2. Restaurar dependencias
dotnet restore

# 3. Ejecutar la API
dotnet run --project CONTACTO-API

# La API queda disponible en:
#   http://localhost:5019
```

### Swagger UI

Mientras la API esté corriendo en modo Development, la documentación interactiva está en:

```
http://localhost:5019/swagger
```

---

## Pruebas

```bash
# Ejecutar todas las pruebas (unitarias + integración)
dotnet test

# Con reporte de cobertura
dotnet test --collect:"XPlat Code Coverage"
```

---

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/v1/contactos` | Lista todos los contactos |
| `GET` | `/api/v1/contactos/{id}` | Obtiene un contacto por ID |
| `POST` | `/api/v1/contactos` | Crea un nuevo contacto |

### Body POST

```json
{
  "nombre": "Leandro Pina",
  "telefono": "123456789"
}
```

### Códigos de respuesta

| Código | Situación |
|---|---|
| `200` | Consulta exitosa |
| `201` | Contacto creado |
| `400` | Datos inválidos (falla de validación) |
| `404` | Contacto no encontrado |
| `409` | Teléfono duplicado |
| `500` | Error interno |

### Formato de error

Todos los errores devuelven el mismo envelope JSON:

```json
{
  "tipo": "validation_error",
  "mensaje": "Los datos enviados no son válidos.",
  "errores": [
    "El nombre es obligatorio.",
    "El teléfono debe tener entre 7 y 20 caracteres."
  ]
}
```

| Campo | Valores posibles de `tipo` |
|---|---|
| `validation_error` | Datos de entrada inválidos (400) |
| `not_found` | Recurso inexistente (404) |
| `conflict` | Teléfono ya registrado (409) |
| `internal_error` | Error no controlado (500) |

---

## Estructura del proyecto

```
CONTACTO/
├── CONTACTO-API/        # Host ASP.NET Core (controladores, middleware, Program.cs)
├── CONTACTO-CORE/       # Lógica de negocio, repositorio en memoria, DTOs
└── CONTACTO-TEST/       # Pruebas unitarias (xUnit + Moq) e integración (WebApplicationFactory)
```
