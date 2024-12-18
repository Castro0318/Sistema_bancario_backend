using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_bancario.Domain.Entities;
using Sistema_bancario.Domain.Filters;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly SisContext _context;

        public ClienteController(SisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ClienteEntity>> GetClientes([FromQuery] ClienteFilter filtro)
        {
            int PageSize = 10;
            if (filtro.Page < 1) filtro.Page = 1;
            int Posicion = (filtro.Page - 1) * PageSize;
            List<Cliente> clientes = await _context.Clientes
                .Include(x => x.IdTipoClienteNavigation)
                .Where(x => x.IdTipoCliente == filtro.IdTipoCliente ||
                filtro.IdTipoCliente == 0 ||
                filtro.IdTipoCliente == default)
                .Skip(Posicion).Take(PageSize)
                .ToListAsync();

            List<ClienteEntity> ClientesRespuesta = new List<ClienteEntity>();
            foreach(Cliente item in clientes)
            {
                ClienteEntity clienteEntity = new ClienteEntity(item);
                ClientesRespuesta.Add(clienteEntity);   
            }

            return ClientesRespuesta;
        }

        [HttpPost]
        public async Task<ActionResult<ClienteEntity>> PostCliente([FromBody] ClienteEntity cliente)
        {
            Cliente clienteNuevo = new Cliente();
            clienteNuevo.NumeroIdentificacionCliente = cliente.NumeroIdentificacionCliente;
            clienteNuevo.NombreCliente = cliente.NombreCliente;
            clienteNuevo.Telefono = cliente.Telefono;
            clienteNuevo.IdTipoCliente = cliente.IdTipoCliente;
            clienteNuevo.RepresentanteLegalNombre = cliente.RepresentanteLegalNombre;
            clienteNuevo.RepresentanteLegalTelefono = cliente.RepresentanteLegalTelefono;
            clienteNuevo.RepresentanteLegalIdentificacion = cliente.RepresentanteLegalIdentificacion;

            _context.Clientes.Add(clienteNuevo);
            await _context.SaveChangesAsync();

            Cliente clienteBuscado = await _context.Clientes
                .Include(t => t.IdTipoClienteNavigation)
                .Where(x => x.IdCliente == clienteNuevo.IdCliente)
                .FirstOrDefaultAsync();

            if(clienteBuscado == null)
            {
                return BadRequest("Cliente no encontrado o agregado incorrectamente");
            }

            ClienteEntity clienteRespuesta = new ClienteEntity(clienteBuscado);

            return clienteRespuesta;
        }

        [HttpPut("{idClienteBuscar}")] //Pide el id del cliente que va a buscar
        public async Task<ActionResult<ClienteEntity>> PutCliente(decimal idClienteBuscar, [FromBody] ClienteEntity cliente)
        {
            Cliente clienteExistente = await _context.Clientes
                .Include(t => t.IdTipoClienteNavigation)
                .Where(x => x.IdCliente == idClienteBuscar)
                .FirstOrDefaultAsync();

            if (clienteExistente == null)
            {
                return NotFound("Cliente no encontrado o no existe");
            } 

            clienteExistente.NumeroIdentificacionCliente = cliente.NumeroIdentificacionCliente;
            clienteExistente.NombreCliente = cliente.NombreCliente;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.IdTipoCliente = cliente.IdTipoCliente;
            clienteExistente.RepresentanteLegalNombre = cliente.RepresentanteLegalNombre;
            clienteExistente.RepresentanteLegalTelefono = cliente.RepresentanteLegalTelefono;
            clienteExistente.RepresentanteLegalIdentificacion = cliente.RepresentanteLegalIdentificacion;

            _context.Clientes.Update(clienteExistente);
            await _context.SaveChangesAsync();

            Cliente clienteActualizado = await _context.Clientes
                .Include(t => t.IdTipoClienteNavigation)
                .Where(x => x.IdCliente == clienteExistente.IdCliente)
                .FirstOrDefaultAsync();

            if (clienteActualizado == null)
            {
                return BadRequest("Error al buscar el cliente");
            }

            ClienteEntity clienteRespuesta = new ClienteEntity(clienteActualizado);

            return clienteRespuesta;
        }

        [HttpDelete("{idClienteBuscar}")] 
        public async Task<ActionResult<ClienteEntity>> DeleteCliente(decimal idClienteBuscar)
        {
            
            Cliente clienteExistente = await _context.Clientes
                .Include(t => t.IdTipoClienteNavigation) 
                .Where(x => x.IdCliente == idClienteBuscar)
                .FirstOrDefaultAsync();

            if (clienteExistente == null)
            {
                return NotFound("Cliente no encontrado o no existe");
            }

            try
            {
                _context.Clientes.Remove(clienteExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
              
                return BadRequest("Error al eliminar el cliente");
            }

            ClienteEntity clienteRespuesta = new ClienteEntity(clienteExistente);

            return clienteRespuesta;
        }

        [HttpGet("{idCliente}")]
        public async Task<ActionResult<ClienteEntity>> GetClientePorId(int idCliente)
        {
            Cliente cliente = await _context.Clientes
                .Include(x => x.IdTipoClienteNavigation) // Incluir la relación con TipoCliente si es necesario
                .Where(x => x.IdCliente == idCliente)
                .FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound("El cliente con ID {idCliente} no existe.");
            }

            ClienteEntity clienteRespuesta = new ClienteEntity(cliente);

            return Ok(clienteRespuesta);
        }




    }
}
