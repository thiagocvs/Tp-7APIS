using Microsoft.AspNetCore.Mvc;
using Ejercicio4api.Entities;

namespace Ejercicio4api.Controllers;

[ApiController]
[Route("[controller]")]
public class MisionesController : ControllerBase
{
    public static List<Mision> misiones = new List<Mision>
    {
        new Mision
        {
            Id = 1,
            DroneId = 1,
            Descripcion = "Inspeccion de torre norte",
            DistanciaKm = 12.5,
            Fecha = new DateTime(2026, 9, 20),
            Completada = true
        },
        new Mision
        {
            Id = 2,
            DroneId = 1,
            Descripcion = "Relevamiento de campo",
            DistanciaKm = 30.0,
            Fecha = new DateTime(2026, 9, 24),
            Completada = true
        },
        new Mision
        {
            Id = 3,
            DroneId = 3,
            Descripcion = "Control de tendido electrico",
            DistanciaKm = 8.2,
            Fecha = new DateTime(2026, 9, 26),
            Completada = true
        }
    };

    // POST -> le asigna una mision a un drone
    [HttpPost]
    public IActionResult Asignar([FromBody] Mision mision)
    {
        try
        {
            if (mision.Descripcion == null)
            {
                return BadRequest("Contame que va a hacer rey, no adivino.");
            }

            var drone = DronesController.drones.FirstOrDefault(d => d.Id == mision.DroneId);

            if (drone == null)
            {
                return NotFound("Ese drone no existe rey.");
            }

            var estaLibre = drone.Estado == "Disponible";

            if (estaLibre == false)
            {
                return BadRequest("Ese drone esta en " + drone.Estado + " rey.");
            }

            if (drone.Bateria < 30)
            {
                return BadRequest("Le queda " + drone.Bateria + " por ciento rey, con menos de 30 no sale.");
            }

            var tieneOtra = misiones.Any(m => m.DroneId == mision.DroneId && m.Completada == false);

            if (tieneOtra)
            {
                return BadRequest("Ese drone ya esta volando rey.");
            }

            if (misiones.Count == 0)
            {
                mision.Id = 1;
            }
            else
            {
                mision.Id = misiones.Max(m => m.Id) + 1;
            }

            mision.Completada = false;
            misiones.Add(mision);

            drone.Estado = "EnMision";

            return Ok(mision);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // PUT -> da la mision por terminada
    [HttpPut("{id:int}/finalizar")]
    public IActionResult Finalizar(int id)
    {
        try
        {
            var mision = misiones.FirstOrDefault(m => m.Id == id);

            if (mision == null)
            {
                return NotFound("No encontre nada rey.");
            }

            if (mision.Completada)
            {
                return BadRequest("Esa mision ya estaba terminada rey.");
            }

            var drone = DronesController.drones.FirstOrDefault(d => d.Id == mision.DroneId);

            if (drone == null)
            {
                return NotFound("Ese drone no existe rey.");
            }

            mision.Completada = true;
            drone.Estado = "Disponible";

            return Ok(mision);
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
            return Ok(misiones);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> las de un drone
    [HttpGet("drone/{droneId:int}")]
    public IActionResult ObtenerPorDrone(int droneId)
    {
        try
        {
            var resultado = misiones
                .Where(m => m.DroneId == droneId)
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

    // GET -> las que faltan terminar, de la mas vieja a la mas nueva
    [HttpGet("pendientes")]
    public IActionResult ObtenerPendientes()
    {
        try
        {
            var resultado = misiones
                .Where(m => m.Completada == false)
                .OrderBy(m => m.Fecha)
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
