using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_bancario.Domain.Filters
{
    public class EstadoCuentaFilter
    {
        public int Page { get; set; }
        public string OrderColumn { get; set; } = "IdEstadoCuenta";
        public string OrderAsdDes { get; set; } = "desc";
       
    }
}
