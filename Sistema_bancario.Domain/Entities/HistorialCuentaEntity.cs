using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Domain.Entities
{
    public class HistorialCuentaEntity
    {
        public int IdHistorialCuenta { get; set; }
        public int IdCuenta { get; set; }
        public double SaldoNuevo { get; set;}
        public double SaldoAnterior { get; set;}
        public DateTime Fecha { get; set; }

        public HistorialCuentaEntity()
        {
            this.IdHistorialCuenta = 0;
            this.IdCuenta = 0;
            this.SaldoNuevo = 0;
            this.SaldoAnterior = 0;
            this.Fecha = DateTime.Now;
        }

        public HistorialCuentaEntity (HistorialCuentum HistorialCuenta)
        {
            this.IdHistorialCuenta = HistorialCuenta.IdHistorialCuenta;
            this.IdCuenta = (int)HistorialCuenta.IdCuenta;
            this.SaldoNuevo = (double)HistorialCuenta.SaldoNuevo;
            this.SaldoAnterior = (double)HistorialCuenta.SaldoAnterior;
        }
    }
}
