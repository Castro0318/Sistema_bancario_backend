using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_bancario.Domain.Filters
{
    public class CuentaFilter
    {
        public int Page { get; set; }
        public int IdTipoCuenta { get; set; } = 0;
        public int IdCliente { get; set; } = 0;
        public int IdEstadoCuenta { get; set; } = 0;
    }
}
