using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_bancario.Domain.Entities;
using Sistema_bancario.Domain.Filters;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialCuentaController : ControllerBase
    {
        private readonly SisContext _context;

        public HistorialCuentaController(SisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<HistorialCuentaEntity>> GetHistorialCuenta([FromQuery] HistorialCuentaFilter filtro)
        {
            int PageSize = 10;
            if (filtro.Page < 1) filtro.Page = 1;
            int Posicion = (filtro.Page - 1) * PageSize;

            List<HistorialCuentum> HistorialCuenta = await _context.HistorialCuenta
                .Include(x => x.IdCuentaNavigation)
                .Where(x => x.IdCuenta == filtro.IdCuenta ||
                filtro.IdCuenta == 0 ||
                filtro.IdCuenta == default)
                .Skip(Posicion).Take(PageSize)
                .ToListAsync();

            List<HistorialCuentaEntity> HistorialRespuesta = new List<HistorialCuentaEntity>()      ;
            foreach (HistorialCuentum item in HistorialCuenta)
            {
                HistorialCuentaEntity HistorialCuentaRespuesta= new HistorialCuentaEntity(item);
                HistorialRespuesta.Add(HistorialCuentaRespuesta);
            }

            return HistorialRespuesta;
        }


        [HttpPost]
        public async Task<ActionResult<HistorialCuentaEntity>> PostCliente([FromBody] HistorialCuentaEntity Historial)
        {
            HistorialCuentum HistorialNuevo = new HistorialCuentum();
            
            HistorialNuevo.IdCuenta = HistorialNuevo.IdCuenta;
            HistorialNuevo.SaldoNuevo = HistorialNuevo.SaldoNuevo;
            HistorialNuevo.SaldoAnterior = HistorialNuevo.SaldoAnterior;
            HistorialNuevo.Fecha = HistorialNuevo.Fecha;

            _context.HistorialCuenta.Add(HistorialNuevo);
            await _context.SaveChangesAsync();

            HistorialCuentum HistorialBuscado = await _context.HistorialCuenta
                .Include(t => t.IdCuentaNavigation)
                .Where(x => x.IdCuenta == HistorialNuevo.IdCuenta)
                .FirstOrDefaultAsync();

            if (HistorialBuscado == null)
            {
                return BadRequest("Historial de cuenta no encontrado o agregado incorrectamente");
            }

            HistorialCuentaEntity HistorialRespuesta = new HistorialCuentaEntity(HistorialBuscado);

            return HistorialRespuesta;
        }

        [HttpPut("{idHistorialCuenta}")]
        public async Task<ActionResult<HistorialCuentaEntity>> PutHistorialCuenta(decimal idHistorialCuenta, [FromBody] HistorialCuentaEntity historial)
        {
            // Buscar el historial existente
            var historialExistente = await _context.HistorialCuenta
                .Where(x => x.IdHistorialCuenta == idHistorialCuenta)
                .FirstOrDefaultAsync();

            if (historialExistente == null)
            {
                return NotFound($"El historial con ID {idHistorialCuenta} no existe.");
            }

            historialExistente.IdCuenta = historial.IdCuenta;
            historialExistente.SaldoNuevo = historial.SaldoNuevo;
            historialExistente.SaldoAnterior = historial.SaldoAnterior;
            historialExistente.Fecha = historial.Fecha;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Error al actualizar el historial. Por favor, inténtalo de nuevo.");
            }

            HistorialCuentaEntity historialRespuesta = new HistorialCuentaEntity(historialExistente);

            return historialRespuesta;
        }

        [HttpDelete("{idHistorialCuenta}")]
        public async Task<ActionResult<HistorialCuentaEntity>> DeleteHistorialCuenta(decimal idHistorialCuenta)
        {
            var historialExistente = await _context.HistorialCuenta
                .Where(x => x.IdHistorialCuenta == idHistorialCuenta)
                .FirstOrDefaultAsync();

            if (historialExistente == null)
            {
                return NotFound($"El historial con ID {idHistorialCuenta} no existe.");
            }

            try
            {
                _context.HistorialCuenta.Remove(historialExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Error al eliminar el historial. Por favor, inténtalo de nuevo.");
            }

            HistorialCuentaEntity historialRespuesta = new HistorialCuentaEntity(historialExistente);

            return historialRespuesta;
        }

        [HttpGet("{idHistorialCuenta}")]
        public async Task<ActionResult<HistorialCuentaEntity>> GetHistorialCuentaPorId(int idHistorialCuenta)
        {
            var historialCuenta = await _context.HistorialCuenta
                .Include(x => x.IdCuentaNavigation) 
                .Where(x => x.IdHistorialCuenta == idHistorialCuenta)
                .FirstOrDefaultAsync();

            if (historialCuenta == null)
            {
                return NotFound("El historial de cuenta con ID {idHistorialCuenta} no existe.");
            }

            HistorialCuentaEntity historialCuentaRespuesta = new HistorialCuentaEntity(historialCuenta);

            return Ok(historialCuentaRespuesta);
        }



    }
}

    

