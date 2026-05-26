# Contactos API

API REST en **.NET 10** para gestión de contactos con almacenamiento en memoria. No requiere base de datos ni configuración adicional.

---

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/v1/contactos` | Lista todos los contactos |
| `GET` | `/api/v1/contactos/{id}` | Obtiene un contacto por ID |
| `POST` | `/api/v1/contactos` | Crea un nuevo contacto |

### Body POST / PUT
```json
{
  "nombre": "Leandro Pina",
  "telefono": "123456789"
}
```

### Códigos de respuesta

| Código | Situación |
|---|---|
| `200` | Consulta o actualización exitosa |
| `201` | Contacto creado |
| `400` | Datos inválidos |
| `404` | Contacto no encontrado |
| `409` | Teléfono duplicado |
| `500` | Error interno |

---

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/es-es/download/dotnet/10.0)
