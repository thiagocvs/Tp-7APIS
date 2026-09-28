using Microsoft.AspNetCore.Mvc;
using Ejercicio1api.Entities;

namespace Ejercicio1api.Controllers;

[ApiController]
[Route("[controller]")]
public class ObjetosPerdidosController : ControllerBase
{
    private static List<ObjetoPerdido> objetos = new List<ObjetoPerdido>
    {
        new ObjetoPerdido
        {
            Id = 1,
            Descripcion = "Mochila azul con cuadernos",
            Categoria = "Equipaje",
            LugarEncontrado = "Plataforma 3",
            FechaEncontrado = new DateTime(2026, 9, 10),
            Reclamado = false,
            NombrePersonaQueRetiro = null
        },
        new ObjetoPerdido
        {
            Id = 2,
            Descripcion = "Celular Samsung negro",
            Categoria = "Electronica",
            LugarEncontrado = "Sala de espera",
            FechaEncontrado = new DateTime(2026, 9, 15),
            Reclamado = true,
            NombrePersonaQueRetiro = "Maria Perez"
        },
        new ObjetoPerdido
        {
            Id = 3,
            Descripcion = "Campera de jean talle M",
            Categoria = "Ropa",
            LugarEncontrado = "Bano de damas",
            FechaEncontrado = new DateTime(2026, 9, 20),
            Reclamado = false,
            NombrePersonaQueRetiro = null
        }
    };

    // POST -> carga uno nuevo
    [HttpPost]
    public IActionResult Registrar([FromBody] ObjetoPerdido objeto)
    {
        try
        {
            if (objeto.Descripcion == null)
            {
                return BadRequest("Escribi algo rey, no adivino.");
            }

            if (objeto.FechaEncontrado > DateTime.Now)
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

            objeto.Reclamado = false;
            objeto.NombrePersonaQueRetiro = null;

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

    // PUT -> le cambia los datos
    [HttpPut("{id:int}")]
    public IActionResult Modificar(int id, [FromBody] ObjetoPerdido objetoNuevo)
    {
        try
        {
            var objeto = objetos.FirstOrDefault(o => o.Id == id);

            if (objeto == null)
            {
                return NotFound("No encontre nada rey.");
            }

            if (objetoNuevo.Descripcion == null)
            {
                return BadRequest("Escribi algo rey, no adivino.");
            }

            if (objetoNuevo.FechaEncontrado > DateTime.Now)
            {
                return BadRequest("Esa fecha todavia no paso rey, dejate de joder.");
            }

            objeto.Descripcion = objetoNuevo.Descripcion;
            objeto.Categoria = objetoNuevo.Categoria;
            objeto.LugarEncontrado = objetoNuevo.LugarEncontrado;
            objeto.FechaEncontrado = objetoNuevo.FechaEncontrado;

            return Ok(objeto);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // DELETE -> chau
    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        try
        {
            var objeto = objetos.FirstOrDefault(o => o.Id == id);

            if (objeto == null)
            {
                return NotFound("No encontre nada rey.");
            }

            objetos.Remove(objeto);
            return Ok("Listo rey, lo borre.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> busca por descripcion
    [HttpGet("buscar")]
    public IActionResult BuscarPorDescripcion([FromBody] string descripcion)
    {
        try
        {
            if (descripcion == null)
            {
                return BadRequest("Pasame algo para buscar rey.");
            }

            var resultado = objetos
                .Where(o => o.Descripcion.ToLower().Contains(descripcion.ToLower()))
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

    // GET -> filtra por categoria
    [HttpGet("categoria/{categoria}")]
    public IActionResult FiltrarPorCategoria(string categoria)
    {
        try
        {
            var resultado = objetos
                .Where(o => o.Categoria.ToLower() == categoria.ToLower())
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

    // GET -> los que nadie retiro
    [HttpGet("no-reclamados")]
    public IActionResult ObtenerNoReclamados()
    {
        try
        {
            var resultado = objetos
                .Where(o => o.Reclamado == false)
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los de despues de una fecha
    [HttpGet("desde")]
    public IActionResult ObtenerDespuesDeFecha([FromBody] DateTime fecha)
    {
        try
        {
            var resultado = objetos
                .Where(o => o.FechaEncontrado > fecha)
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> del mas nuevo al mas viejo
    [HttpGet("recientes")]
    public IActionResult ObtenerOrdenadosPorFecha()
    {
        try
        {
            var resultado = objetos
                .OrderByDescending(o => o.FechaEncontrado)
                .ToList();

            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // PUT -> alguien lo viene a buscar
    [HttpPut("{id:int}/reclamar")]
    public IActionResult Reclamar(int id, [FromBody] string nombrePersona)
    {
        try
        {
            var objeto = objetos.FirstOrDefault(o => o.Id == id);

            if (objeto == null)
            {
                return NotFound("No encontre nada rey.");
            }

            if (nombrePersona == null)
            {
                return BadRequest("Decime quien lo retira rey.");
            }

            if (objeto.Reclamado)
            {
                return BadRequest("Llegaste tarde rey, se lo llevo " + objeto.NombrePersonaQueRetiro + ".");
            }

            objeto.Reclamado = true;
            objeto.NombrePersonaQueRetiro = nombrePersona;

            return Ok(objeto);
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }

    // GET -> los numeritos
    [HttpGet("resumen")]
    public IActionResult ObtenerResumen()
    {
        try
        {
            var total = objetos.Count;
            var reclamados = objetos.Count(o => o.Reclamado);
            var sinReclamar = objetos.Count(o => o.Reclamado == false);

            return Ok(new
            {
                Total = total,
                Reclamados = reclamados,
                SinReclamar = sinReclamar
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Se rompio todo rey.");
        }
    }
}
