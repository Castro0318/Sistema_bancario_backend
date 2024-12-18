using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_bancario.Domain.Entities;
using Sistema_bancario.Domain.Filters;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoCuentaController : ControllerBase
    {
        private readonly SisContext _context;

        public EstadoCuentaController(SisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<EstadoCuentaEntity>> GetEstadoCuenta([FromQuery] EstadoCuentaFilter filtro)
        {
            int PageSize = 10;
            if (filtro.Page < 1) filtro.Page = 1;
            int Posicion = (filtro.Page - 1) * PageSize;

            List<EstadoCuentum> EstadoCuentas = await _context.EstadoCuenta
                .Skip(Posicion).Take(PageSize)
                .ToListAsync();

            List<EstadoCuentaEntity> EstadoRespuesta = new List<EstadoCuentaEntity>();
            foreach (EstadoCuentum item in EstadoCuentas)
            {
                EstadoCuentaEntity EstadoCuentaRespuesta = new EstadoCuentaEntity(item);
                EstadoRespuesta.Add(EstadoCuentaRespuesta);
            }

            return EstadoRespuesta;
        }

        [HttpPost]
        public async Task<ActionResult<EstadoCuentaEntity>> PostEstadoCuenta([FromBody] EstadoCuentaEntity estadoCuenta)
        {
            EstadoCuentum estadoCuentaNuevo = new EstadoCuentum();
            estadoCuentaNuevo.Nombre = estadoCuenta.Nombre;

            _context.EstadoCuenta.Add(estadoCuentaNuevo);
            await _context.SaveChangesAsync();

            EstadoCuentum estadoCuentaBuscado = await _context.EstadoCuenta
                .Where(x => x.IdEstadoCuenta == estadoCuentaNuevo.IdEstadoCuenta)
                .FirstOrDefaultAsync();

            if (estadoCuentaBuscado == null)
            {
                return BadRequest("Estado de cuenta no encontrado o agregado incorrectamente");
            }

            EstadoCuentaEntity estadoCuentaRespuesta = new EstadoCuentaEntity(estadoCuentaBuscado);

            return estadoCuentaRespuesta;
        }

        [HttpPut("{idEstadoCuenta}")]
        public async Task<ActionResult<EstadoCuentaEntity>> PutEstadoCuenta(int idEstadoCuenta, [FromBody] EstadoCuentaEntity estadoCuenta)
        {
            EstadoCuentum estadoExistente = await _context.EstadoCuenta
                .Where(x => x.IdEstadoCuenta == idEstadoCuenta)
                .FirstOrDefaultAsync();

            if (estadoExistente == null)
            {
                return NotFound($"El estado de cuenta con ID {idEstadoCuenta} no existe.");
            }

            estadoExistente.Nombre = estadoCuenta.Nombre;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Error al actualizar el estado de cuenta. Por favor, inténtalo de nuevo.");
            }

            EstadoCuentaEntity estadoCuentaRespuesta = new EstadoCuentaEntity(estadoExistente);

            return Ok(estadoCuentaRespuesta);
        }

        [HttpDelete("{idEstadoCuenta}")]
        public async Task<ActionResult<EstadoCuentaEntity>> DeleteEstadoCuenta(int idEstadoCuenta)
        {
            EstadoCuentum estadoExistente = await _context.EstadoCuenta
                .Where(x => x.IdEstadoCuenta == idEstadoCuenta)
                .FirstOrDefaultAsync();

            if (estadoExistente == null)
            {
                return NotFound($"El estado de cuenta con ID {idEstadoCuenta} no existe.");
            }

            try
            {
                _context.EstadoCuenta.Remove(estadoExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Error al eliminar el estado de cuenta. Por favor, inténtalo de nuevo.");
            }

            EstadoCuentaEntity estadoCuentaRespuesta = new EstadoCuentaEntity(estadoExistente);

            return Ok(estadoCuentaRespuesta);
        }

        [HttpGet("{idEstadoCuenta}")]
        public async Task<ActionResult<EstadoCuentaEntity>> GetEstadoCuentaPorId(int idEstadoCuenta)
        {
            var estadoCuenta = await _context.EstadoCuenta
                .Where(x => x.IdEstadoCuenta == idEstadoCuenta)
                .FirstOrDefaultAsync();

            if (estadoCuenta == null)
            {
                return NotFound("El estado de cuenta con ID {idEstadoCuenta} no existe.");
            }

            EstadoCuentaEntity estadoCuentaRespuesta = new EstadoCuentaEntity(estadoCuenta);

            return Ok(estadoCuentaRespuesta);
        }



    }
}
