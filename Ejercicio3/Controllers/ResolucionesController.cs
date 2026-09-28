using Microsoft.AspNetCore.Mvc;
using Ejercicio3api.Entities;

namespace Ejercicio3api.Controllers;

[ApiController]
[Route("[controller]")]
public class ResolucionesController : ControllerBase
{
    public static List<Resolucion> resoluciones = new List<Resolucion>
    {
        new Resolucion
        {
            Id = 1,
            ParticipanteId = 1,
            DesafioId = 1,
            PuntajeObtenido = 45,
            FechaEntrega = new DateTime(2026, 9, 20)
        },
        new Resolucion
        {
            Id = 2,
            ParticipanteId = 1,
            DesafioId = 2,
            PuntajeObtenido = 80,
            FechaEntrega = new DateTime(2026, 9, 22)
        },
        new Resolucion
        {
            Id = 3,
            ParticipanteId = 2,
            DesafioId = 1,
            PuntajeObtenido = 30,
            FechaEntrega = new DateTime(2026, 9, 23)
        }
    };

    // POST -> carga la resolucion de un desafio
    [HttpPost]
    public IActionResult Registrar([FromBody] Resolucion resolucion)
    {
        try
        {
            var participante = ParticipantesController.participantes
                .FirstOrDefault(p => p.Id == resolucion.ParticipanteId);

            if (participante == null)
            {
                return NotFound("Ese participante no existe rey.");
            }

            var desafio = DesafiosController.desafios
                .FirstOrDefault(d => d.Id == resolucion.DesafioId);

            if (desafio == null)
            {
                return NotFound("Ese desafio no existe rey.");
            }

            if (desafio.Activo == false)
            {
                return BadRequest("Ese desafio esta cerrado rey.");
            }

            var yaLoHizo = resoluciones.Any(r => r.ParticipanteId == resolucion.ParticipanteId
                && r.DesafioId == resolucion.DesafioId);

            if (yaLoHizo)
            {
                return BadRequest("Ya lo resolviste rey, una sola vez.");
            }

            if (resolucion.PuntajeObtenido > desafio.PuntajeMaximo)
            {
                return BadRequest("Te pasaste rey, el maximo es " + desafio.PuntajeMaximo + ".");
            }

            if (resoluciones.Count == 0)
            {
                resolucion.Id = 1;
            }
            else
            {
                resolucion.Id = resoluciones.Max(r => r.Id) + 1;
            }

            resoluciones.Add(resolucion);
            return Ok(resolucion);
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
            return Ok(resoluciones);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> las de un participante
    [HttpGet("participante/{participanteId:int}")]
    public IActionResult ObtenerPorParticipante(int participanteId)
    {
        try
        {
            var resultado = resoluciones
                .Where(r => r.ParticipanteId == participanteId)
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
