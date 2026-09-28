using Microsoft.AspNetCore.Mvc;
using Ejercicio5api.Entities;

namespace Ejercicio5api.Controllers;

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
            Edad = 22
        },
        new Participante
        {
            Id = 2,
            Nombre = "Lucia Fernandez",
            Email = "lucia@mail.com",
            Edad = 25
        },
        new Participante
        {
            Id = 3,
            Nombre = "Martin Rios",
            Email = "martin@mail.com",
            Edad = 30
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

            if (participante.Edad <= 0)
            {
                return BadRequest("Esa edad no existe rey.");
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

    // GET -> en que se anoto
    [HttpGet("{id:int}/actividades")]
    public IActionResult ObtenerActividades(int id)
    {
        try
        {
            var participante = participantes.FirstOrDefault(p => p.Id == id);

            if (participante == null)
            {
                return NotFound("Ese participante no existe rey.");
            }

            var anotado = ReservasController.reservas
                .Where(r => r.ParticipanteId == id)
                .Select(r => r.ActividadId)
                .ToList();

            var resultado = ActividadesController.actividades
                .Where(a => anotado.Contains(a.Id))
                .ToList();

            if (resultado.Count == 0)
            {
                return NotFound("No se anoto en nada rey.");
            }

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
