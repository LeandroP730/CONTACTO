using CONTACTO_CORE.Domain.Response;
using CONTACTO_CORE.Dto;
using CONTACTO_CORE.Services;
using Microsoft.AspNetCore.Mvc;

namespace CONTACTO_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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


        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var response = _contactoService.ObtenerPorId(id);

            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Value);
        }


        [HttpPost]
        public IActionResult Create([FromBody] CrearContactoRequest request)
        {
            var response = _contactoService.Crear(request);

            if (!response.IsSuccess)
            {
                if (response.ErrorType == ResultErrorType.Conflict)
                {
                    return Conflict(response.ErrorMessage);
                }

                return BadRequest(response.ErrorMessage);
            }

            return Created($"api/contactos/{response.Value!.Id}", response.Value);
        }
    }
}
