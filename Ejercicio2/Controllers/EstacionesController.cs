using Microsoft.AspNetCore.Mvc;
using Ejercicio2api.Entities;

namespace Ejercicio2api.Controllers;

[ApiController]
[Route("[controller]")]
public class EstacionesController : ControllerBase
{
    public static List<Estacion> estaciones = new List<Estacion>
    {
        new Estacion
        {
            Id = 1,
            Nombre = "Estacion Centro",
            Localidad = "Rosario",
            Activa = true
        },
        new Estacion
        {
            Id = 2,
            Nombre = "Estacion Norte",
            Localidad = "Rosario",
            Activa = true
        },
        new Estacion
        {
            Id = 3,
            Nombre = "Estacion Vieja",
            Localidad = "Funes",
            Activa = false
        }
    };

    // POST -> carga una estacion
    [HttpPost]
    public IActionResult Registrar([FromBody] Estacion estacion)
    {
        try
        {
            if (estacion.Nombre == null)
            {
                return BadRequest("Ponele un nombre rey, no adivino.");
            }

            if (estacion.Localidad == null)
            {
                return BadRequest("Decime de que localidad es rey.");
            }

            if (estaciones.Count == 0)
            {
                estacion.Id = 1;
            }
            else
            {
                estacion.Id = estaciones.Max(e => e.Id) + 1;
            }

            estaciones.Add(estacion);
            return Ok(estacion);
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
            return Ok(estaciones);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> una por id
    [HttpGet("{id:int}")]
    public IActionResult ObtenerPorId(int id)
    {
        try
        {
            var estacion = estaciones.FirstOrDefault(e => e.Id == id);

            if (estacion == null)
            {
                return NotFound("No encontre nada rey.");
            }

            return Ok(estacion);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> busca por localidad
    [HttpGet("localidad/{localidad}")]
    public IActionResult BuscarPorLocalidad(string localidad)
    {
        try
        {
            var resultado = estaciones
                .Where(e => e.Localidad.ToLower() == localidad.ToLower())
                .ToList();

            if (resultado.Count == 0)
            {
                return NotFound("No encontre nada rey.");
            }

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
