using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace ObituariosWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObituariosController : ControllerBase
    {
        private readonly FirebirdService _firebirdService;

        public ObituariosController(FirebirdService firebirdService)
        {
            _firebirdService = firebirdService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var tabla = _firebirdService.ObtenerHomenajes();

                var lista = tabla.AsEnumerable()
                    .Select(row => new
                    {
                        servicioId = row["COD_SERVICIO_SEPELIO"],
                        establecimiento = row["ESTABLECIMIENTO"],
                        sala = row["SALA"],
                        fallecido = row["FALLECIDO"],
                        dni = row["DNI"],
                        fechaFallecimiento = row["FECHA_FALLECIMIENTO"],
                        inicioServicio = row["INICIO_SERVICIO"],
                        photo = "https://via.placeholder.com/150",
                        summary = "Siempre te recordaremos con cariño."
                    })
                    .ToList();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    mensaje = "Modo demo (sin conexión a DB)",
                    error = ex.Message,
                    data = new[]
                    {
                        new {
                            fallecido = "Juan Pérez",
                            fecha = "2026-03-20"
                        }
                    }
                });
            }
        }
    }
}