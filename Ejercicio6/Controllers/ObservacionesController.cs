using Microsoft.AspNetCore.Mvc;
using Ejercicio6api.Entities;

namespace Ejercicio6api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ObservacionesController : ControllerBase
{
    public static List<Observacion> observaciones = new List<Observacion>
    {
        new Observacion
        {
            Id = 1,
            ObjetoEspacialId = 1,
            Fecha = new DateTime(2026, 9, 10),
            DistanciaMedida = 31.2,
            Velocidad = 30.7,
            Comentario = "Trayectoria estable"
        },
        new Observacion
        {
            Id = 2,
            ObjetoEspacialId = 1,
            Fecha = new DateTime(2026, 9, 20),
            DistanciaMedida = 30.9,
            Velocidad = 31.1,
            Comentario = "Se acerca un poco"
        },
        new Observacion
        {
            Id = 3,
            ObjetoEspacialId = 3,
            Fecha = new DateTime(2026, 9, 22),
            DistanciaMedida = 0.54,
            Velocidad = 7.6,
            Comentario = "Orbita baja"
        }
    };

    // POST -> carga una observacion
    [HttpPost]
    public IActionResult Registrar([FromBody] Observacion observacion)
    {
        try
        {
            var objeto = ObjetosEspacialesController.objetos
                .FirstOrDefault(o => o.Id == observacion.ObjetoEspacialId);

            if (objeto == null)
            {
                return NotFound("Ese objeto no existe rey.");
            }

            if (observacion.DistanciaMedida < 0)
            {
                return BadRequest("Distancia negativa no existe rey.");
            }

            if (observacion.Velocidad < 0)
            {
                return BadRequest("Velocidad negativa no existe rey.");
            }

            if (observacion.Fecha > DateTime.Now)
            {
                return BadRequest("Esa fecha todavia no paso rey, dejate de joder.");
            }

            if (observaciones.Count == 0)
            {
                observacion.Id = 1;
            }
            else
            {
                observacion.Id = observaciones.Max(o => o.Id) + 1;
            }

            observaciones.Add(observacion);
            return Ok(observacion);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> todas
    [HttpGet]
    public IActionResult ObtenerTodas()
    {
        try
        {
            return Ok(observaciones);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> las de un objeto
    [HttpGet("objeto/{objetoEspacialId:int}")]
    public IActionResult ObtenerPorObjeto(int objetoEspacialId)
    {
        try
        {
            var objeto = ObjetosEspacialesController.objetos
                .FirstOrDefault(o => o.Id == objetoEspacialId);

            if (objeto == null)
            {
                return NotFound("Ese objeto no existe rey.");
            }

            var resultado = observaciones
                .Where(o => o.ObjetoEspacialId == objetoEspacialId)
                .ToList();

            if (resultado.Count == 0)
            {
                return NotFound("Todavia no lo miro nadie rey.");
            }

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> la velocidad promedio
    [HttpGet("velocidad-promedio")]
    public IActionResult ObtenerVelocidadPromedio()
    {
        try
        {
            if (observaciones.Count == 0)
            {
                return NotFound("No encontre nada rey.");
            }

            var promedio = observaciones.Average(o => o.Velocidad);

            return Ok(new { VelocidadPromedio = Math.Round(promedio, 2) });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
