using Microsoft.AspNetCore.Mvc;
using Ejercicio4api.Entities;

namespace Ejercicio4api.Controllers;

[ApiController]
[Route("[controller]")]
public class DronesController : ControllerBase
{
    public static List<Drone> drones = new List<Drone>
    {
        new Drone
        {
            Id = 1,
            Codigo = "DR-001",
            Modelo = "Phantom 4",
            Bateria = 90,
            Estado = "Disponible"
        },
        new Drone
        {
            Id = 2,
            Codigo = "DR-002",
            Modelo = "Mavic Air",
            Bateria = 25,
            Estado = "Disponible"
        },
        new Drone
        {
            Id = 3,
            Codigo = "DR-003",
            Modelo = "Phantom 4",
            Bateria = 60,
            Estado = "Mantenimiento"
        }
    };

    // POST -> carga un drone
    [HttpPost]
    public IActionResult Registrar([FromBody] Drone drone)
    {
        try
        {
            if (drone.Codigo == null)
            {
                return BadRequest("Ponele un codigo rey, no adivino.");
            }

            if (drone.Modelo == null)
            {
                return BadRequest("Decime que modelo es rey.");
            }

            if (drone.Bateria < 0 || drone.Bateria > 100)
            {
                return BadRequest("La bateria va de 0 a 100 rey.");
            }

            if (drone.Estado == null)
            {
                drone.Estado = "Disponible";
            }

            var estado = drone.Estado.ToLower();

            if (estado == "disponible")
            {
                drone.Estado = "Disponible";
            }
            else if (estado == "enmision")
            {
                drone.Estado = "EnMision";
            }
            else if (estado == "mantenimiento")
            {
                drone.Estado = "Mantenimiento";
            }
            else
            {
                return BadRequest("Ese estado no existe rey, va Disponible, EnMision o Mantenimiento.");
            }

            if (drones.Count == 0)
            {
                drone.Id = 1;
            }
            else
            {
                drone.Id = drones.Max(d => d.Id) + 1;
            }

            drones.Add(drone);
            return Ok(drone);
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
            return Ok(drones);
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
            var drone = drones.FirstOrDefault(d => d.Id == id);

            if (drone == null)
            {
                return NotFound("No encontre nada rey.");
            }

            return Ok(drone);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los que estan libres
    [HttpGet("disponibles")]
    public IActionResult ObtenerDisponibles()
    {
        try
        {
            var resultado = drones
                .Where(d => d.Estado == "Disponible")
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

    // GET -> los que estan con poca bateria
    [HttpGet("bateria-menor-a")]
    public IActionResult ObtenerPorBateriaMenorA([FromQuery] int porcentaje)
    {
        try
        {
            var resultado = drones
                .Where(d => d.Bateria < porcentaje)
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

    // GET -> busca por modelo
    [HttpGet("modelo/{modelo}")]
    public IActionResult BuscarPorModelo(string modelo)
    {
        try
        {
            var resultado = drones
                .Where(d => d.Modelo.ToLower() == modelo.ToLower())
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

    // GET -> cuanto volo cada uno
    [HttpGet("distancia-total")]
    public IActionResult ObtenerDistanciaTotal()
    {
        try
        {
            var resultado = drones
                .Select(d => new
                {
                    DroneId = d.Id,
                    Codigo = d.Codigo,
                    KilometrosTotales = MisionesController.misiones
                        .Where(m => m.DroneId == d.Id)
                        .Sum(m => m.DistanciaKm)
                })
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> el que mas volo
    [HttpGet("mas-kilometros")]
    public IActionResult ObtenerElQueMasVolo()
    {
        try
        {
            var ranking = drones
                .Select(d => new
                {
                    DroneId = d.Id,
                    Codigo = d.Codigo,
                    KilometrosTotales = MisionesController.misiones
                        .Where(m => m.DroneId == d.Id)
                        .Sum(m => m.DistanciaKm)
                })
                .OrderByDescending(d => d.KilometrosTotales)
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

    // GET -> la bateria promedio de los libres
    [HttpGet("bateria-promedio")]
    public IActionResult ObtenerBateriaPromedio()
    {
        try
        {
            var disponibles = drones
                .Where(d => d.Estado == "Disponible")
                .ToList();

            if (disponibles.Count == 0)
            {
                return NotFound("No encontre nada rey.");
            }

            var promedio = disponibles.Average(d => d.Bateria);

            return Ok(new { BateriaPromedio = promedio });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
