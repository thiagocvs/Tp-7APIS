using Microsoft.AspNetCore.Mvc;
using Ejercicio2api.Entities;

namespace Ejercicio2api.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicionesController : ControllerBase
{
    private static List<Medicion> mediciones = new List<Medicion>
    {
        new Medicion
        {
            Id = 1,
            EstacionId = 1,
            Temperatura = 28.5,
            Humedad = 60,
            VelocidadViento = 12,
            FechaHora = new DateTime(2026, 9, 25, 8, 0, 0)
        },
        new Medicion
        {
            Id = 2,
            EstacionId = 1,
            Temperatura = 33.2,
            Humedad = 45,
            VelocidadViento = 20,
            FechaHora = new DateTime(2026, 9, 25, 14, 0, 0)
        },
        new Medicion
        {
            Id = 3,
            EstacionId = 2,
            Temperatura = 19.8,
            Humedad = 80,
            VelocidadViento = 5,
            FechaHora = new DateTime(2026, 9, 26, 7, 30, 0)
        }
    };

    // POST -> carga una medicion
    [HttpPost]
    public IActionResult Registrar([FromBody] Medicion medicion)
    {
        try
        {
            var existe = EstacionesController.estaciones.Any(e => e.Id == medicion.EstacionId);

            if (existe == false)
            {
                return NotFound("Esa estacion no existe rey.");
            }

            var estacion = EstacionesController.estaciones.First(e => e.Id == medicion.EstacionId);

            if (estacion.Activa == false)
            {
                return BadRequest("Esa estacion esta apagada rey, no mide nada.");
            }

            if (medicion.Humedad < 0 || medicion.Humedad > 100)
            {
                return BadRequest("La humedad va de 0 a 100 rey.");
            }

            if (medicion.VelocidadViento < 0)
            {
                return BadRequest("Viento negativo no existe rey.");
            }

            if (medicion.FechaHora > DateTime.Now)
            {
                return BadRequest("Esa fecha todavia no paso rey, dejate de joder.");
            }

            if (mediciones.Count == 0)
            {
                medicion.Id = 1;
            }
            else
            {
                medicion.Id = mediciones.Max(m => m.Id) + 1;
            }

            mediciones.Add(medicion);
            return Ok(medicion);
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
            return Ok(mediciones);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> las de una estacion
    [HttpGet("estacion/{estacionId:int}")]
    public IActionResult ObtenerPorEstacion(int estacionId)
    {
        try
        {
            var resultado = mediciones
                .Where(m => m.EstacionId == estacionId)
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

    // GET -> las que pasan de una temperatura
    [HttpGet("temperatura-mayor-a")]
    public IActionResult ObtenerPorTemperaturaMayorA([FromQuery] double temperatura)
    {
        try
        {
            var resultado = mediciones
                .Where(m => m.Temperatura > temperatura)
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

    // GET -> de la mas calurosa para abajo
    [HttpGet("mas-calurosas")]
    public IActionResult ObtenerOrdenadasPorTemperatura()
    {
        try
        {
            var resultado = mediciones
                .OrderByDescending(m => m.Temperatura)
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> el promedio
    [HttpGet("temperatura-promedio")]
    public IActionResult ObtenerTemperaturaPromedio()
    {
        try
        {
            if (mediciones.Count == 0)
            {
                return NotFound("No encontre nada rey.");
            }

            var promedio = mediciones.Average(m => m.Temperatura);

            return Ok(new { TemperaturaPromedio = promedio });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> la mas alta
    [HttpGet("temperatura-maxima")]
    public IActionResult ObtenerTemperaturaMaxima()
    {
        try
        {
            if (mediciones.Count == 0)
            {
                return NotFound("No encontre nada rey.");
            }

            var maxima = mediciones.Max(m => m.Temperatura);

            return Ok(new { TemperaturaMaxima = maxima });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> la mas baja
    [HttpGet("temperatura-minima")]
    public IActionResult ObtenerTemperaturaMinima()
    {
        try
        {
            if (mediciones.Count == 0)
            {
                return NotFound("No encontre nada rey.");
            }

            var minima = mediciones.Min(m => m.Temperatura);

            return Ok(new { TemperaturaMinima = minima });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> cuantas hizo cada estacion
    [HttpGet("conteo-por-estacion")]
    public IActionResult ContarPorEstacion()
    {
        try
        {
            var resultado = EstacionesController.estaciones
                .Select(e => new
                {
                    EstacionId = e.Id,
                    Nombre = e.Nombre,
                    Cantidad = mediciones.Count(m => m.EstacionId == e.Id)
                })
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
