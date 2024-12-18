using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_bancario.Domain.Entities;
using Sistema_bancario.Domain.Filters;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoCuentaController : ControllerBase
    {
        private readonly SisContext _context;

        public TipoCuentaController(SisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<TipoCuentaEntity>> GetTipoCuenta([FromQuery] TipoCuentaFilter filtro)
        {
            int PageSize = 10;
            if (filtro.Page < 1) filtro.Page = 1;
            int Posicion = (filtro.Page - 1) * PageSize;

            List<TipoCuentum> TipoCuenta = await _context.TipoCuenta
                .Skip(Posicion).Take(PageSize)
                .ToListAsync();

            List<TipoCuentaEntity> TipoRespuesta = new List<TipoCuentaEntity>();
            foreach (TipoCuentum item in TipoCuenta)
            {
                TipoCuentaEntity TipoCuentaEntity = new TipoCuentaEntity(item);
                TipoRespuesta.Add(TipoCuentaEntity);
            }

            return TipoRespuesta;
        }

        [HttpPost]
        public async Task<ActionResult<TipoCuentaEntity>> PostCliente([FromBody] TipoCuentaEntity Tipocuenta)
        {
            TipoCuentum TipocuentaNuevo = new TipoCuentum();
            TipocuentaNuevo.NombreTipoCuenta = Tipocuenta.NombreTipoCuenta;


            _context.TipoCuenta.Add(TipocuentaNuevo);
            await _context.SaveChangesAsync();

            TipoCuentum TipocuentaBuscado = await _context.TipoCuenta
                .Where(x => x.IdTipoCuenta == TipocuentaNuevo.IdTipoCuenta)
                .FirstOrDefaultAsync();

            if (TipocuentaBuscado == null)
            {
                return BadRequest("Cliente no encontrado o agregado incorrectamente");
            }

            TipoCuentaEntity TipocuentaRespuesta = new TipoCuentaEntity(TipocuentaBuscado);

            return TipocuentaRespuesta;
        }

        [HttpPut("{idTipoCuenta}")]
        public async Task<ActionResult<TipoCuentaEntity>> PutTipoCuenta(int idTipoCuenta, [FromBody] TipoCuentaEntity tipoCuenta)
        {
            if (tipoCuenta == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            // Buscar el TipoCuenta existente en la base de datos
            TipoCuentum tipoCuentaExistente = await _context.TipoCuenta
                .Where(x => x.IdTipoCuenta == idTipoCuenta)
                .FirstOrDefaultAsync();

            if (tipoCuentaExistente == null)
            {
                return NotFound($"El tipo de cuenta con ID {idTipoCuenta} no existe.");
            }

            // Actualizar los valores
            tipoCuentaExistente.NombreTipoCuenta = tipoCuenta.NombreTipoCuenta;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Otro usuario modificó este recurso. Por favor, recarga los datos y vuelve a intentar.");
            }

            return Ok(tipoCuentaExistente);
        }

        [HttpDelete("{idTipoCuenta}")]
        public async Task<IActionResult> DeleteTipoCuenta(int idTipoCuenta)
        {
            TipoCuentum tipoCuentaExistente = await _context.TipoCuenta
                .Where(x => x.IdTipoCuenta == idTipoCuenta)
                .FirstOrDefaultAsync();

            if (tipoCuentaExistente == null)
            {
                return NotFound($"El tipo de cuenta con ID {idTipoCuenta} no existe.");
            }

            _context.TipoCuenta.Remove(tipoCuentaExistente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el recurso: {ex.Message}");
            }

            return NoContent(); 
        }

        [HttpGet("{idTipoCuenta}")]
        public async Task<ActionResult<TipoCuentaEntity>> GetTipoCuentaPorId(int idTipoCuenta)
        {
            var tipoCuenta = await _context.TipoCuenta
                .Where(x => x.IdTipoCuenta == idTipoCuenta)
                .FirstOrDefaultAsync();

            if (tipoCuenta == null)
            {
                return NotFound("El tipo de cuenta con ID {idTipoCuenta} no existe.");
            }

            TipoCuentaEntity tipoCuentaRespuesta = new TipoCuentaEntity(tipoCuenta);

            return Ok(tipoCuentaRespuesta);
        }




    }
}
   