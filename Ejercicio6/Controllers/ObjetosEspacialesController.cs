using Microsoft.AspNetCore.Mvc;
using Ejercicio6api.Entities;

namespace Ejercicio6api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ObjetosEspacialesController : ControllerBase
{
    public static List<ObjetoEspacial> objetos = new List<ObjetoEspacial>
    {
        new ObjetoEspacial
        {
            Id = 1,
            Nombre = "Apophis",
            Tipo = "Asteroide",
            Distancia = 31.0,
            NivelRiesgo = 5,
            FechaDescubrimiento = new DateTime(2004, 6, 19),
            Activo = true
        },
        new ObjetoEspacial
        {
            Id = 2,
            Nombre = "Cometa Halley",
            Tipo = "Cometa",
            Distancia = 5400.0,
            NivelRiesgo = 1,
            FechaDescubrimiento = new DateTime(1758, 12, 25),
            Activo = true
        },
        new ObjetoEspacial
        {
            Id = 3,
            Nombre = "Starlink 1130",
            Tipo = "Satelite",
            Distancia = 0.55,
            NivelRiesgo = 2,
            FechaDescubrimiento = new DateTime(2020, 1, 7),
            Activo = true
        },
        new ObjetoEspacial
        {
            Id = 4,
            Nombre = "Objeto X-17",
            Tipo = "Desconocido",
            Distancia = 18.4,
            NivelRiesgo = 4,
            FechaDescubrimiento = new DateTime(2026, 3, 2),
            Activo = false
        }
    };

    // POST -> carga un objeto
    [HttpPost]
    public IActionResult Registrar([FromBody] ObjetoEspacial objeto)
    {
        try
        {
            if (objeto.Nombre == null)
            {
                return BadRequest("Ponele un nombre rey, no adivino.");
            }

            if (objeto.Tipo == null)
            {
                return BadRequest("Decime que tipo de objeto es rey.");
            }

            if (objeto.Distancia < 0)
            {
                return BadRequest("Distancia negativa no existe rey.");
            }

            if (objeto.NivelRiesgo < 1 || objeto.NivelRiesgo > 5)
            {
                return BadRequest("El riesgo va de 1 a 5 rey.");
            }

            if (objeto.FechaDescubrimiento > DateTime.Now)
            {
                return BadRequest("Esa fecha todavia no paso rey, dejate de joder.");
            }

            if (objetos.Count == 0)
            {
                objeto.Id = 1;
            }
            else
            {
                objeto.Id = objetos.Max(o => o.Id) + 1;
            }

            objetos.Add(objeto);
            return Ok(objeto);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> todos
    [HttpGet]
    public IActionResult ObtenerTodos()
    {
        try
        {
            return Ok(objetos);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> uno por id
    [HttpGet("{id:int}")]
    public IActionResult ObtenerPorId(int id)
    {
        try
        {
            var objeto = objetos.FirstOrDefault(o => o.Id == id);

            if (objeto == null)
            {
                return NotFound("No encontre nada rey.");
            }

            return Ok(objeto);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> busca por nombre
    [HttpGet("buscar")]
    public IActionResult BuscarPorNombre([FromBody] string nombre)
    {
        try
        {
            if (nombre == null)
            {
                return BadRequest("Pasame algo para buscar rey.");
            }

            var resultado = objetos
                .Where(o => o.Nombre.ToLower().Contains(nombre.ToLower()))
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

    // GET -> filtra por tipo
    [HttpGet("tipo/{tipo}")]
    public IActionResult FiltrarPorTipo(string tipo)
    {
        try
        {
            var resultado = objetos
                .Where(o => o.Tipo.ToLower() == tipo.ToLower())
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

    // GET -> filtra por nivel de riesgo
    [HttpGet("riesgo/{nivel:int}")]
    public IActionResult FiltrarPorRiesgo(int nivel)
    {
        try
        {
            var resultado = objetos
                .Where(o => o.NivelRiesgo == nivel)
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

    // GET -> los que estan mas cerca que tanto
    [HttpGet("distancia-menor-a")]
    public IActionResult ObtenerPorDistanciaMenorA([FromQuery] double distancia)
    {
        try
        {
            var resultado = objetos
                .Where(o => o.Distancia < distancia)
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

    // GET -> el que mas miraron
    [HttpGet("mas-observado")]
    public IActionResult ObtenerElMasObservado()
    {
        try
        {
            var ranking = objetos
                .Select(o => new
                {
                    Id = o.Id,
                    Nombre = o.Nombre,
                    Tipo = o.Tipo,
                    NivelRiesgo = o.NivelRiesgo,
                    Observaciones = ObservacionesController.observaciones.Count(x => x.ObjetoEspacialId == o.Id)
                })
                .OrderByDescending(o => o.Observaciones)
                .ToList();

            if (ranking.Count == 0)
            {
                return NotFound("No encontre nada rey.");
            }

            return Ok(ranking.First());
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los que nadie miro todavia
    [HttpGet("sin-observaciones")]
    public IActionResult ObtenerSinObservaciones()
    {
        try
        {
            var resultado = objetos
                .Where(o => ObservacionesController.observaciones.Any(x => x.ObjetoEspacialId == o.Id) == false)
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

    // GET -> del mas peligroso al mas tranquilo
    [HttpGet("por-riesgo")]
    public IActionResult OrdenarPorRiesgo()
    {
        try
        {
            var resultado = objetos
                .OrderByDescending(o => o.NivelRiesgo)
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los numeritos del sistema
    [HttpGet("estadisticas")]
    public IActionResult ObtenerEstadisticas()
    {
        try
        {
            double distanciaPromedio = 0;
            double riesgoPromedio = 0;

            if (objetos.Count > 0)
            {
                distanciaPromedio = Math.Round(objetos.Average(o => o.Distancia), 2);
                riesgoPromedio = Math.Round(objetos.Average(o => o.NivelRiesgo), 2);
            }

            double velocidadPromedio = 0;

            if (ObservacionesController.observaciones.Count > 0)
            {
                velocidadPromedio = Math.Round(ObservacionesController.observaciones.Average(o => o.Velocidad), 2);
            }

            var porTipo = objetos
                .GroupBy(o => o.Tipo)
                .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
                .ToList();

            var sinObservaciones = objetos
                .Count(o => ObservacionesController.observaciones.Any(x => x.ObjetoEspacialId == o.Id) == false);

            return Ok(new
            {
                TotalObjetos = objetos.Count,
                Activos = objetos.Count(o => o.Activo),
                Inactivos = objetos.Count(o => o.Activo == false),
                TotalObservaciones = ObservacionesController.observaciones.Count,
                ObjetosSinObservaciones = sinObservaciones,
                DistanciaPromedio = distanciaPromedio,
                RiesgoPromedio = riesgoPromedio,
                VelocidadPromedio = velocidadPromedio,
                PorTipo = porTipo
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los que hay que mirar de cerca
    [HttpGet("alertas")]
    public IActionResult ObtenerAlertas([FromQuery] int riesgoMinimo = 4, [FromQuery] double distanciaMaxima = 50)
    {
        try
        {
            var resultado = objetos
                .Where(o => o.NivelRiesgo >= riesgoMinimo && o.Distancia < distanciaMaxima && o.Activo)
                .OrderByDescending(o => o.NivelRiesgo)
                .ToList();

            if (resultado.Count == 0)
            {
                return NotFound("Dormi tranquilo rey, no hay alertas.");
            }

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
