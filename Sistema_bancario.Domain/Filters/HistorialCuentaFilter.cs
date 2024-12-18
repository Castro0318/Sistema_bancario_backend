using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_bancario.Domain.Filters
{
    public class HistorialCuentaFilter
    {
        public int Page { get; set; }
        public string OrderColumn { get; set; } = "IdHistorialCuenta";
        public string OrderAsdDes { get; set; } = "desc";
        public int IdCuenta { get; set; } = 0;
    }
}
