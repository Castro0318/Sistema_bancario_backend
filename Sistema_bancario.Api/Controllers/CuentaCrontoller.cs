using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_bancario.Domain.Entities;
using Sistema_bancario.Domain.Filters;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly SisContext _context;

        public CuentaController(SisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<CuentaEntity>> GetCuentas([FromQuery] CuentaFilter filtro)
        {
            int PageSize = 10;
            if (filtro.Page < 1) filtro.Page = 1;
            int Posicion = (filtro.Page - 1) * PageSize;

            //Pro en el modelo lo llamo cuentum 
            List<Cuentum> cuentas = await _context.Cuenta //Aca lo lla mo cuenta que es el contexto
                .Include(x => x.IdTipoCuentaNavigation)
                .Include(x => x.IdClienteNavigation)
                .Include(x => x.IdEstadoCuentaNavigation)
                .Where(x => x.IdTipoCuenta == filtro.IdTipoCuenta ||
                filtro.IdTipoCuenta == 0 ||
                filtro.IdTipoCuenta == default)
                .Where(x => x.IdCliente == filtro.IdCliente ||
                filtro.IdCliente == 0 ||
                filtro.IdCliente == default)
                .Where(x => x.IdEstadoCuenta == filtro.IdEstadoCuenta ||
                filtro.IdEstadoCuenta == 0 ||
                filtro.IdEstadoCuenta == default)
                .Skip(Posicion).Take(PageSize)
                .ToListAsync();

            List<CuentaEntity> CuentaRespuesta = new List<CuentaEntity>();
            foreach (Cuentum item in cuentas)
            {
                CuentaEntity cuentaEntity = new CuentaEntity(item);
                CuentaRespuesta.Add(cuentaEntity);
            }

            return CuentaRespuesta;
        }

        [HttpGet("~/api/Cuenta/GetSaldoTotalCuentas{IdCliente}")]
        public async Task<ActionResult<double>> GetSaldoTotalCuentas(int IdCliente)
        {

            List<Cuentum> Saldocuentas = await _context.Cuenta
                .Include(x => x.IdTipoCuentaNavigation)
                .Include(x => x.IdClienteNavigation)
                .Include(x => x.IdEstadoCuentaNavigation)
                .Where(X => X.IdCliente == IdCliente)
                .ToListAsync();
                
            if (Saldocuentas.Count == 0)
            {
                return BadRequest("El cleinte no tiene cuentas");
            }

            double SaldoRespuesta = 0;

            foreach(Cuentum item in Saldocuentas)
            {
                SaldoRespuesta = SaldoRespuesta + (double)item.Saldo;
            }

            return SaldoRespuesta;
        }


        [HttpPost]
        public async Task<ActionResult<CuentaEntity>> PostCuenta([FromBody] CuentaEntity cuenta)
        {

            List<Cuentum> cuentasCliente = await _context.Cuenta
                .Where(x => x.IdCliente == cuenta.IdCliente)
                .ToListAsync();

            if(cuentasCliente != null)
            {
                foreach(Cuentum item in cuentasCliente)
                {
                    if(item.IdTipoCuenta == cuenta.IdTipoCuenta)
                    {
                        return BadRequest("El cliente ya tiene una cuenta de este tipo");
                    }
                }
            }

            Cuentum CuentaNueva = new Cuentum();
            CuentaNueva.IdTipoCuenta = cuenta.IdTipoCuenta;
            CuentaNueva.IdCliente = cuenta.IdCliente;
            CuentaNueva.IdEstadoCuenta = cuenta.IdEstadoCuenta;
            CuentaNueva.Saldo = cuenta.Saldo;

            if (CuentaNueva.IdTipoCuenta == 1)
            {
                CuentaNueva.InteresCuenta = (double) 0.4;
            }

            if (CuentaNueva.IdTipoCuenta == 2)
            {
                CuentaNueva.InteresCuenta = (double) 0;
            }

            if (CuentaNueva.IdTipoCuenta == 3)
            {
                CuentaNueva.InteresCuenta = cuenta.InteresCuenta; 
            }

            _context.Cuenta.Add(CuentaNueva);
            await _context.SaveChangesAsync();

            Cuentum cuentaBuscada = await _context.Cuenta
                .Include(x => x.IdTipoCuentaNavigation)
                .Include(x => x.IdClienteNavigation)
                .Include(x => x.IdEstadoCuentaNavigation)
                .Where(x => x.IdCuenta == CuentaNueva.IdCuenta)
                .FirstOrDefaultAsync();

            if (cuentaBuscada == null)
            {
                return BadRequest("Cuenta no encontrado o agregado incorrectamente");
            }

            CuentaEntity cuentaRespuesta = new CuentaEntity(cuentaBuscada);

            return cuentaRespuesta;
        }

        [HttpPut("~/api/Cuenta/Depositar")]
        public async Task<ActionResult<CuentaEntity>> PutDepositarCuenta(decimal idCuentaBuscar, double Deposito)
        {
            Cuentum cuentaExistente = await _context.Cuenta
         .Where(x => x.IdCuenta == idCuentaBuscar)
         .FirstOrDefaultAsync();

            if (cuentaExistente == null)
            {
                return NotFound($"La cuenta con ID {idCuentaBuscar} no existe.");
            }

            if (cuentaExistente.IdTipoCuenta == 3)
            {
                return BadRequest("No se permite hacer depositos o retiros a un CDT");
            }

            cuentaExistente.Saldo = cuentaExistente.Saldo + Deposito;

            try
            {
                _context.Entry(cuentaExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Error al depositar dinero.");
            }


            Cuentum cuentaBuscada = await _context.Cuenta
                .Include(x => x.IdTipoCuentaNavigation)
                .Include(x => x.IdClienteNavigation)
                .Include(x => x.IdEstadoCuentaNavigation)
                .Where(x => x.IdCuenta == idCuentaBuscar)
                .FirstOrDefaultAsync();

            CuentaEntity cuentaRespuesta = new CuentaEntity(cuentaBuscada);

            return cuentaRespuesta;
        }

        [HttpPut("~/api/Cuenta/Retirar")]
        public async Task<ActionResult<CuentaEntity>> PutRetirarCuenta(decimal idCuentaBuscar, double Retiro)
        {
            Cuentum cuentaExistente = await _context.Cuenta
         .Where(x => x.IdCuenta == idCuentaBuscar)
         .FirstOrDefaultAsync();

            //Validar que la cuenta existe
            if (cuentaExistente == null)
            {
                return NotFound($"La cuenta con ID {idCuentaBuscar} no existe.");
            }
            //Validar que la cuenta no sea un CDT
            if (cuentaExistente.IdTipoCuenta == 3)
            {
                return BadRequest("No se permite Retirar de un CDT");
            }

            //validar que la cuenta tenga dinero

            if (cuentaExistente.Saldo == 0) 
            {
                return BadRequest("Fondos insuficientes");
            }

            //valida que la cuenta tenga fondos para hacer un retiro
            else if (cuentaExistente.Saldo - Retiro < 0)
            {
                return BadRequest("Fondos insuficientes");
            }

            cuentaExistente.Saldo = cuentaExistente.Saldo -  Retiro;


            try
            {
                _context.Entry(cuentaExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Error al Retirar dinero.");
            }

            Cuentum cuentaBuscada = await _context.Cuenta
                .Include(x => x.IdTipoCuentaNavigation)
                .Include(x => x.IdClienteNavigation)
                .Include(x => x.IdEstadoCuentaNavigation)
                .Where(x => x.IdCuenta == idCuentaBuscar)
                .FirstOrDefaultAsync();

            CuentaEntity cuentaRespuesta = new CuentaEntity(cuentaBuscada);

            return cuentaRespuesta;
        }



        [HttpPut("{idCuentaBuscar}")] 
        public async Task<ActionResult<CuentaEntity>> PutCuenta(decimal idCuentaBuscar, [FromBody] CuentaEntity cuenta)
        {
            var cuentaExistente = await _context.Cuenta
         .Where(x => x.IdCuenta == idCuentaBuscar)
         .FirstOrDefaultAsync();

            if (cuentaExistente == null)
            {
                return NotFound($"La cuenta con ID {idCuentaBuscar} no existe.");
            }

            cuentaExistente.IdTipoCuenta = cuenta.IdTipoCuenta;
            cuentaExistente.IdCliente = cuenta.IdCliente;
            cuentaExistente.IdEstadoCuenta = cuenta.IdEstadoCuenta;
            cuentaExistente.Saldo = cuenta.Saldo;
            cuentaExistente.InteresCuenta = cuenta.InteresCuenta;

            if (cuentaExistente.Saldo == 3)
            {

            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Error al actualizar la cuenta. Por favor, inténtalo de nuevo.");
   
         }

            Cuentum cuentaBuscada = await _context.Cuenta
                .Include(x => x.IdTipoCuentaNavigation)
                .Include(x => x.IdClienteNavigation)
                .Include(x => x.IdEstadoCuentaNavigation)
                .Where(x => x.IdCuenta == idCuentaBuscar)
                .FirstOrDefaultAsync();

            CuentaEntity cuentaRespuesta = new CuentaEntity(cuentaBuscada);

            return cuentaRespuesta;
        }

        [HttpDelete("{idCuentaBuscar}")]
        public async Task<ActionResult<CuentaEntity>> DeleteCuenta(decimal idCuentaBuscar)
        {
            Cuentum cuentaExistente = await _context.Cuenta
                .Include(t => t.IdTipoCuentaNavigation)
                .Where(x => x.IdCuenta == idCuentaBuscar)
                .FirstOrDefaultAsync();

            if (cuentaExistente == null)
            {
                return NotFound($"La cuenta con ID {idCuentaBuscar} no existe.");
            }

            try
            {
                _context.Cuenta.Remove(cuentaExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Error al eliminar la cuenta. Por favor, inténtalo de nuevo.");
            }

            CuentaEntity cuentaRespuesta = new CuentaEntity(cuentaExistente);

            return cuentaRespuesta;
        }

        [HttpGet("{idCuenta}")]
        public async Task<ActionResult<CuentaEntity>> GetCuentaPorId(decimal idCuenta)
        {
            var cuenta = await _context.Cuenta 
                .Include(x => x.IdTipoCuentaNavigation) 
                .Include(x => x.IdClienteNavigation)   
                .Include(x => x.IdEstadoCuentaNavigation) 
                .Where(x => x.IdCuenta == idCuenta) 
                .FirstOrDefaultAsync();

            if (cuenta == null)
            {
                return NotFound("La cuenta con ID {idCuenta} no existe.");
            }

            CuentaEntity cuentaRespuesta = new CuentaEntity(cuenta);

            return Ok(cuentaRespuesta);
        }

        [HttpDelete("~/api/Cuenta/CancelarCDT/{idEstadoCuenta}")]
        public async Task<ActionResult> CancelarCDT(decimal idEstadoCuenta)
        {
            // Buscar la cuenta CDT
            var cuentaCDT = await _context.Cuenta
                .Include(x => x.IdTipoCuentaNavigation)
                .Where(x => x.IdCuenta == idEstadoCuenta)
                .Where(x => x.IdTipoCuenta == 3)// Validar que es un CDT
                .FirstOrDefaultAsync();

            if (cuentaCDT == null)
            {
                return NotFound($"El CDT con ID {idEstadoCuenta} no existe.");
            }

            // Buscar una cuenta de ahorros del cliente
            var cuentaAhorros = await _context.Cuenta
                .Where(x => x.IdCliente == cuentaCDT.IdCliente)
                .Where(x => x.IdTipoCuenta == 1) // Tipo 2 = Cuenta de ahorros
                .FirstOrDefaultAsync();

            if (cuentaAhorros == null)
            {
                return BadRequest("El cliente no tiene una cuenta de ahorros. Por favor, cree una cuenta de ahorros primero.");
            }
            // Transferir el saldo total a la cuenta de ahorros
            cuentaAhorros.Saldo += cuentaAhorros.Saldo + cuentaCDT.Saldo;

            try
            {
                // Eliminar el CDT
                _context.Cuenta.Remove(cuentaCDT);

                // Actualizar la cuenta de ahorros
                _context.Entry(cuentaAhorros).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Ocurrió un error al cancelar el CDT. Por favor, inténtelo nuevamente.");
            }

            return Ok($"El CDT con ID {idEstadoCuenta} fue cancelado exitosamente. El saldo total ({cuentaAhorros.Saldo}) fue transferido a la cuenta de ahorros con ID {cuentaAhorros.IdCuenta}.");
        }




    }








}
