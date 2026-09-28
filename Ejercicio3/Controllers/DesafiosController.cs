using Microsoft.AspNetCore.Mvc;
using Ejercicio3api.Entities;

namespace Ejercicio3api.Controllers;

[ApiController]
[Route("[controller]")]
public class DesafiosController : ControllerBase
{
    public static List<Desafio> desafios = new List<Desafio>
    {
        new Desafio
        {
            Id = 1,
            Titulo = "FizzBuzz",
            Dificultad = "Facil",
            PuntajeMaximo = 50,
            Activo = true
        },
        new Desafio
        {
            Id = 2,
            Titulo = "Ordenar una lista",
            Dificultad = "Media",
            PuntajeMaximo = 100,
            Activo = true
        },
        new Desafio
        {
            Id = 3,
            Titulo = "Camino minimo en un grafo",
            Dificultad = "Dificil",
            PuntajeMaximo = 200,
            Activo = false
        }
    };

    // POST -> crea un desafio
    [HttpPost]
    public IActionResult Crear([FromBody] Desafio desafio)
    {
        try
        {
            if (desafio.Titulo == null)
            {
                return BadRequest("Ponele un titulo rey, no adivino.");
            }

            if (desafio.Dificultad == null)
            {
                return BadRequest("Decime que tan dificil es rey.");
            }

            if (desafio.PuntajeMaximo <= 0)
            {
                return BadRequest("El puntaje maximo tiene que ser mayor a cero rey.");
            }

            if (desafios.Count == 0)
            {
                desafio.Id = 1;
            }
            else
            {
                desafio.Id = desafios.Max(d => d.Id) + 1;
            }

            desafios.Add(desafio);
            return Ok(desafio);
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
            return Ok(desafios);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los que estan abiertos
    [HttpGet("activos")]
    public IActionResult ObtenerActivos()
    {
        try
        {
            var resultado = desafios
                .Where(d => d.Activo)
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

    // GET -> busca por dificultad
    [HttpGet("dificultad/{dificultad}")]
    public IActionResult BuscarPorDificultad(string dificultad)
    {
        try
        {
            var resultado = desafios
                .Where(d => d.Dificultad.ToLower() == dificultad.ToLower())
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

    // GET -> el promedio de puntajes de un desafio
    [HttpGet("{id:int}/promedio")]
    public IActionResult ObtenerPromedio(int id)
    {
        try
        {
            var desafio = desafios.FirstOrDefault(d => d.Id == id);

            if (desafio == null)
            {
                return NotFound("Ese desafio no existe rey.");
            }

            var resoluciones = ResolucionesController.resoluciones
                .Where(r => r.DesafioId == id)
                .ToList();

            if (resoluciones.Count == 0)
            {
                return NotFound("Todavia no lo resolvio nadie rey.");
            }

            var promedio = resoluciones.Average(r => r.PuntajeObtenido);

            return Ok(new
            {
                DesafioId = desafio.Id,
                Titulo = desafio.Titulo,
                PuntajePromedio = promedio
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
