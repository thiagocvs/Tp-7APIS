using Microsoft.AspNetCore.Mvc;
using Ejercicio5api.Entities;

namespace Ejercicio5api.Controllers;

[ApiController]
[Route("[controller]")]
public class ActividadesController : ControllerBase
{
    public static List<Actividad> actividades = new List<Actividad>
    {
        new Actividad
        {
            Id = 1,
            Nombre = "Introduccion a la IA",
            Tipo = "Charla",
            Horario = new DateTime(2026, 10, 10, 10, 0, 0),
            DuracionMinutos = 60,
            Capacidad = 3,
            Activa = true
        },
        new Actividad
        {
            Id = 2,
            Nombre = "Taller de Arduino",
            Tipo = "Taller",
            Horario = new DateTime(2026, 10, 10, 10, 30, 0),
            DuracionMinutos = 90,
            Capacidad = 2,
            Activa = true
        },
        new Actividad
        {
            Id = 3,
            Nombre = "Competencia de algoritmos",
            Tipo = "Competencia",
            Horario = new DateTime(2026, 10, 10, 14, 0, 0),
            DuracionMinutos = 120,
            Capacidad = 2,
            Activa = true
        },
        new Actividad
        {
            Id = 4,
            Nombre = "Demo de robotica",
            Tipo = "Demostracion",
            Horario = new DateTime(2026, 10, 10, 17, 0, 0),
            DuracionMinutos = 45,
            Capacidad = 5,
            Activa = false
        }
    };

    // POST -> crea una actividad
    [HttpPost]
    public IActionResult Crear([FromBody] Actividad actividad)
    {
        try
        {
            if (actividad.Nombre == null)
            {
                return BadRequest("Ponele un nombre rey, no adivino.");
            }

            if (actividad.Tipo == null)
            {
                return BadRequest("Decime que tipo de actividad es rey.");
            }

            if (actividad.DuracionMinutos <= 0)
            {
                return BadRequest("La duracion tiene que ser mayor a cero rey.");
            }

            if (actividad.Capacidad <= 0)
            {
                return BadRequest("La capacidad tiene que ser mayor a cero rey.");
            }

            if (actividades.Count == 0)
            {
                actividad.Id = 1;
            }
            else
            {
                actividad.Id = actividades.Max(a => a.Id) + 1;
            }

            actividades.Add(actividad);
            return Ok(actividad);
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
            return Ok(actividades);
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
            var actividad = actividades.FirstOrDefault(a => a.Id == id);

            if (actividad == null)
            {
                return NotFound("No encontre nada rey.");
            }

            return Ok(actividad);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> las que estan abiertas
    [HttpGet("disponibles")]
    public IActionResult ObtenerDisponibles()
    {
        try
        {
            var resultado = actividades
                .Where(a => a.Activa)
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

    // GET -> busca por nombre
    [HttpGet("buscar")]
    public IActionResult BuscarPorNombre([FromQuery] string nombre)
    {
        try
        {
            if (nombre == null)
            {
                return BadRequest("Pasame algo para buscar rey.");
            }

            var resultado = actividades
                .Where(a => a.Nombre.ToLower().Contains(nombre.ToLower()))
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
            var resultado = actividades
                .Where(a => a.Tipo.ToLower() == tipo.ToLower())
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

    // GET -> las que todavia tienen lugar
    [HttpGet("con-lugar")]
    public IActionResult ObtenerConLugar()
    {
        try
        {
            var resultado = actividades
                .Where(a => ReservasController.reservas.Count(r => r.ActividadId == a.Id) < a.Capacidad)
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

    // GET -> las que ya se llenaron
    [HttpGet("completas")]
    public IActionResult ObtenerCompletas()
    {
        try
        {
            var resultado = actividades
                .Where(a => ReservasController.reservas.Count(r => r.ActividadId == a.Id) >= a.Capacidad)
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

    // GET -> de la mas llena a la mas vacia
    [HttpGet("por-inscriptos")]
    public IActionResult OrdenarPorInscriptos()
    {
        try
        {
            var resultado = actividades
                .Select(a => new
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Tipo = a.Tipo,
                    Capacidad = a.Capacidad,
                    Inscriptos = ReservasController.reservas.Count(r => r.ActividadId == a.Id)
                })
                .OrderByDescending(a => a.Inscriptos)
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> la que mas gente junto
    [HttpGet("mas-inscriptos")]
    public IActionResult ObtenerLaDeMasInscriptos()
    {
        try
        {
            var ranking = actividades
                .Select(a => new
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Tipo = a.Tipo,
                    Capacidad = a.Capacidad,
                    Inscriptos = ReservasController.reservas.Count(r => r.ActividadId == a.Id)
                })
                .OrderByDescending(a => a.Inscriptos)
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

    // GET -> que tan llena esta cada una
    [HttpGet("ocupacion")]
    public IActionResult ObtenerOcupacion()
    {
        try
        {
            var resultado = actividades
                .Select(a => new
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Capacidad = a.Capacidad,
                    Inscriptos = ReservasController.reservas.Count(r => r.ActividadId == a.Id),
                    PorcentajeOcupacion = Math.Round(ReservasController.reservas.Count(r => r.ActividadId == a.Id) * 100.0 / a.Capacidad, 2)
                })
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> quienes se anotaron
    [HttpGet("{id:int}/participantes")]
    public IActionResult ObtenerParticipantes(int id)
    {
        try
        {
            var actividad = actividades.FirstOrDefault(a => a.Id == id);

            if (actividad == null)
            {
                return NotFound("Esa actividad no existe rey.");
            }

            var anotados = ReservasController.reservas
                .Where(r => r.ActividadId == id)
                .Select(r => r.ParticipanteId)
                .ToList();

            var resultado = ParticipantesController.participantes
                .Where(p => anotados.Contains(p.Id))
                .ToList();

            if (resultado.Count == 0)
            {
                return NotFound("Todavia no se anoto nadie rey.");
            }

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
