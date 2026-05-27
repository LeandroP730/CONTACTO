using CONTACTO_API.Models;
using CONTACTO_CORE.Domain.Response;
using CONTACTO_CORE.Dto;
using CONTACTO_CORE.Services;
using Microsoft.AspNetCore.Mvc;

namespace CONTACTO_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactoService _contactoService;

        public ContactosController(IContactoService contactoService)
        {
            _contactoService = contactoService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var contactos = _contactoService.Obtener();
            return Ok(contactos);
        }

        [HttpGet("{id:int}")]
        public IActionResult ObtenerPorId(int id)
        {
            var result = _contactoService.ObtenerPorId(id);

            if (!result.IsSuccess)
            {
                return NotFound(new ErrorResponse
                {
                    Tipo = "not_found",
                    Mensaje = result.ErrorMessage!
                });
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CrearContactoRequest request)
        {
            var result = _contactoService.Crear(request);

            if (!result.IsSuccess)
            {
                if (result.ErrorType == ResultErrorType.Conflict)
                {
                    return Conflict(new ErrorResponse
                    {
                        Tipo = "conflict",
                        Mensaje = result.ErrorMessage!
                    });
                }

                return BadRequest(new ErrorResponse
                {
                    Tipo = "bad_request",
                    Mensaje = result.ErrorMessage!
                });
            }

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = result.Value!.Id },
                result.Value
            );
        }
    }
}
