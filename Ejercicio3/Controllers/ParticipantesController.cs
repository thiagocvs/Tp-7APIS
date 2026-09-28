using Microsoft.AspNetCore.Mvc;
using Ejercicio3api.Entities;

namespace Ejercicio3api.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipantesController : ControllerBase
{
    public static List<Participante> participantes = new List<Participante>
    {
        new Participante
        {
            Id = 1,
            Nombre = "Tomas Gonzalez",
            Email = "tomas@mail.com",
            Nivel = "Avanzado"
        },
        new Participante
        {
            Id = 2,
            Nombre = "Lucia Fernandez",
            Email = "lucia@mail.com",
            Nivel = "Intermedio"
        },
        new Participante
        {
            Id = 3,
            Nombre = "Martin Rios",
            Email = "martin@mail.com",
            Nivel = "Principiante"
        }
    };

    // POST -> carga un participante
    [HttpPost]
    public IActionResult Registrar([FromBody] Participante participante)
    {
        try
        {
            if (participante.Nombre == null)
            {
                return BadRequest("Ponele un nombre rey, no adivino.");
            }

            if (participante.Email == null)
            {
                return BadRequest("Sin mail no entra rey.");
            }

            var mailRepetido = participantes.Any(p => p.Email.ToLower() == participante.Email.ToLower());

            if (mailRepetido)
            {
                return BadRequest("Ese mail ya lo tiene otro rey.");
            }

            if (participantes.Count == 0)
            {
                participante.Id = 1;
            }
            else
            {
                participante.Id = participantes.Max(p => p.Id) + 1;
            }

            participantes.Add(participante);
            return Ok(participante);
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
            return Ok(participantes);
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
            var participante = participantes.FirstOrDefault(p => p.Id == id);

            if (participante == null)
            {
                return NotFound("No encontre nada rey.");
            }

            return Ok(participante);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los que pasaron cierto puntaje
    [HttpGet("mayores-a")]
    public IActionResult ObtenerPorPuntajeMayorA([FromQuery] int puntaje)
    {
        try
        {
            var resultado = participantes
                .Select(p => new
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    PuntajeTotal = ResolucionesController.resoluciones
                        .Where(r => r.ParticipanteId == p.Id)
                        .Sum(r => r.PuntajeObtenido)
                })
                .Where(p => p.PuntajeTotal > puntaje)
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

    // GET -> el que mas puntos junto
    [HttpGet("mayor-puntaje")]
    public IActionResult ObtenerElDeMayorPuntaje()
    {
        try
        {
            var ranking = participantes
                .Select(p => new
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    PuntajeTotal = ResolucionesController.resoluciones
                        .Where(r => r.ParticipanteId == p.Id)
                        .Sum(r => r.PuntajeObtenido)
                })
                .OrderByDescending(p => p.PuntajeTotal)
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

    // GET -> tabla de posiciones
    [HttpGet("ranking")]
    public IActionResult ObtenerRanking()
    {
        try
        {
            var resultado = participantes
                .Select(p => new
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    PuntajeTotal = ResolucionesController.resoluciones
                        .Where(r => r.ParticipanteId == p.Id)
                        .Sum(r => r.PuntajeObtenido)
                })
                .OrderByDescending(p => p.PuntajeTotal)
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los desafios que le faltan
    [HttpGet("{id:int}/pendientes")]
    public IActionResult ObtenerDesafiosPendientes(int id)
    {
        try
        {
            var participante = participantes.FirstOrDefault(p => p.Id == id);

            if (participante == null)
            {
                return NotFound("Ese participante no existe rey.");
            }

            var resueltos = ResolucionesController.resoluciones
                .Where(r => r.ParticipanteId == id)
                .Select(r => r.DesafioId)
                .ToList();

            var resultado = DesafiosController.desafios
                .Where(d => resueltos.Contains(d.Id) == false)
                .ToList();

            if (resultado.Count == 0)
            {
                return NotFound("Ya los hizo todos rey.");
            }

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
