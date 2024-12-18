using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Domain.Entities
{
    public class TipoCuentaEntity
    {
        public int IdTipoCuenta { get; set; }
        public string? NombreTipoCuenta { get; set; }

        public TipoCuentaEntity()
        {
            this.IdTipoCuenta = 0;
            this.NombreTipoCuenta = string.Empty;
        }

        public TipoCuentaEntity (TipoCuentum tipocuenta)
        {
            this.IdTipoCuenta = tipocuenta.IdTipoCuenta;
            this.NombreTipoCuenta = tipocuenta.NombreTipoCuenta;
        }
    }
}