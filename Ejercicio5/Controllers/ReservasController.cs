using Microsoft.AspNetCore.Mvc;
using Ejercicio5api.Entities;

namespace Ejercicio5api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservasController : ControllerBase
{
    public static List<ReservaActividad> reservas = new List<ReservaActividad>
    {
        new ReservaActividad
        {
            Id = 1,
            ActividadId = 1,
            ParticipanteId = 1,
            FechaReserva = new DateTime(2026, 9, 25)
        },
        new ReservaActividad
        {
            Id = 2,
            ActividadId = 1,
            ParticipanteId = 2,
            FechaReserva = new DateTime(2026, 9, 25)
        },
        new ReservaActividad
        {
            Id = 3,
            ActividadId = 3,
            ParticipanteId = 3,
            FechaReserva = new DateTime(2026, 9, 26)
        }
    };

    // POST -> anota a alguien en una actividad
    [HttpPost]
    public IActionResult Inscribir([FromBody] ReservaActividad reserva)
    {
        try
        {
            var participante = ParticipantesController.participantes
                .FirstOrDefault(p => p.Id == reserva.ParticipanteId);

            if (participante == null)
            {
                return NotFound("Ese participante no existe rey.");
            }

            var actividad = ActividadesController.actividades
                .FirstOrDefault(a => a.Id == reserva.ActividadId);

            if (actividad == null)
            {
                return NotFound("Esa actividad no existe rey.");
            }

            if (actividad.Activa == false)
            {
                return BadRequest("Esa actividad esta cerrada rey.");
            }

            var yaEstaAnotado = reservas.Any(r => r.ActividadId == reserva.ActividadId
                && r.ParticipanteId == reserva.ParticipanteId);

            if (yaEstaAnotado)
            {
                return BadRequest("Ya estas anotado en esa rey.");
            }

            var ocupados = reservas.Count(r => r.ActividadId == reserva.ActividadId);

            if (ocupados >= actividad.Capacidad)
            {
                return BadRequest("Esa actividad ya esta llena rey.");
            }

            var inicioNueva = actividad.Horario;
            var finNueva = actividad.Horario.AddMinutes(actividad.DuracionMinutos);

            var anotado = reservas
                .Where(r => r.ParticipanteId == reserva.ParticipanteId)
                .Select(r => r.ActividadId)
                .ToList();

            var seSuperpone = ActividadesController.actividades
                .Where(a => anotado.Contains(a.Id))
                .Any(a => inicioNueva < a.Horario.AddMinutes(a.DuracionMinutos) && a.Horario < finNueva);

            if (seSuperpone)
            {
                var choque = ActividadesController.actividades
                    .Where(a => anotado.Contains(a.Id))
                    .First(a => inicioNueva < a.Horario.AddMinutes(a.DuracionMinutos) && a.Horario < finNueva);

                return BadRequest("Se te pisa con " + choque.Nombre + " rey.");
            }

            if (reservas.Count == 0)
            {
                reserva.Id = 1;
            }
            else
            {
                reserva.Id = reservas.Max(r => r.Id) + 1;
            }

            reserva.FechaReserva = DateTime.Now;

            reservas.Add(reserva);
            return Ok(reserva);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // DELETE -> lo baja de la actividad
    [HttpDelete("{id:int}")]
    public IActionResult Cancelar(int id)
    {
        try
        {
            var reserva = reservas.FirstOrDefault(r => r.Id == id);

            if (reserva == null)
            {
                return NotFound("No encontre nada rey.");
            }

            reservas.Remove(reserva);
            return Ok("Listo rey, lo baje.");
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
            return Ok(reservas);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
