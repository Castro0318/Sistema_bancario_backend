using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_bancario.Domain.Entities;
using Sistema_bancario.Domain.Filters;
using Sistema_bancario.Infrastructure.Models;
namespace Sistema_bancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoClienteController : ControllerBase
    {
        private readonly SisContext _context;

        public TipoClienteController(SisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<TipoClienteEntity>> GetEstadoCuenta([FromQuery] TipoClienteFilter filtro)
        {
            int PageSize = 10;
            if (filtro.Page < 1) filtro.Page = 1;
            int Posicion = (filtro.Page - 1) * PageSize;

            List<TipoCliente> TipoClientes = await _context.TipoClientes
                .Skip(Posicion).Take(PageSize)
                .ToListAsync();

            List<TipoClienteEntity> ClientesRespuesta = new List<TipoClienteEntity>();
            foreach (TipoCliente item in TipoClientes)
            {
                TipoClienteEntity TipoClientesRespuesta = new TipoClienteEntity(item);
                ClientesRespuesta.Add(TipoClientesRespuesta);
            }

            return ClientesRespuesta;
        }

        [HttpPost]
        public async Task<ActionResult<TipoClienteEntity>> PostCliente([FromBody] TipoClienteEntity Tipocliente)
        {
            TipoCliente TipoclienteNuevo = new TipoCliente();
            TipoclienteNuevo.NombreTipoCliente = Tipocliente.NombreTipoCliente;
           

            _context.TipoClientes.Add(TipoclienteNuevo);
            await _context.SaveChangesAsync();

            TipoCliente TipoclienteBuscado = await _context.TipoClientes
                .Where(x => x.IdTipoCliente == TipoclienteNuevo.IdTipoCliente)
                .FirstOrDefaultAsync();

            if (TipoclienteBuscado == null)
            {
                return BadRequest("Cliente no encontrado o agregado incorrectamente");
            }

            TipoClienteEntity TipoclienteRespuesta = new TipoClienteEntity(TipoclienteBuscado);

            return TipoclienteRespuesta;
        }

        [HttpPut("{idTipoCliente}")]
        public async Task<ActionResult<TipoClienteEntity>> PutCliente(int id, [FromBody] TipoClienteEntity Tipocliente)
        {
            TipoCliente TipoclienteExistente = await _context.TipoClientes
                .Where(x => x.IdTipoCliente == id)
                .FirstOrDefaultAsync();

            if (TipoclienteExistente == null)
            {
                return NotFound("TipoCliente no encontrado.");
            }

            TipoclienteExistente.NombreTipoCliente = Tipocliente.NombreTipoCliente;

            _context.TipoClientes.Update(TipoclienteExistente);
            await _context.SaveChangesAsync();

            TipoClienteEntity TipoclienteRespuesta = new TipoClienteEntity(TipoclienteExistente);

            return Ok(TipoclienteRespuesta);
        }

        [HttpDelete("{idTipoCliente}")]
        public async Task<ActionResult<TipoClienteEntity>> DeleteTipoCliente(int idTipoCliente)
        {
            TipoCliente tipoClienteExistente = await _context.TipoClientes
                .Where(x => x.IdTipoCliente == idTipoCliente)
                .FirstOrDefaultAsync();

            if (tipoClienteExistente == null)
            {
                return NotFound($"El TipoCliente con ID {idTipoCliente} no existe.");
            }

            try
            {
                _context.TipoClientes.Remove(tipoClienteExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Error al eliminar el TipoCliente. Por favor, inténtalo de nuevo.");
            }
            
            TipoClienteEntity tipoClienteRespuesta = new TipoClienteEntity(tipoClienteExistente);

            return Ok(tipoClienteRespuesta);
        }

        [HttpGet("{idTipoCliente}")]
        public async Task<ActionResult<TipoClienteEntity>> GetTipoClientePorId(int idTipoCliente)
        {
            var tipoCliente = await _context.TipoClientes
                .Where(x => x.IdTipoCliente == idTipoCliente)
                .FirstOrDefaultAsync();

            if (tipoCliente == null)
            {
                return NotFound("El tipo de cliente con ID {idTipoCliente} no existe.");
            }

            TipoClienteEntity tipoClienteRespuesta = new TipoClienteEntity(tipoCliente);

            return Ok(tipoClienteRespuesta);
        }






    }
}

    
