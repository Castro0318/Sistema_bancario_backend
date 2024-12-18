using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_bancario.Domain.Filters
{
    public class TipoCuentaFilter
    {
        public int Page { get; set; }
        public string OrderColumn { get; set; } = "IdTipoCuenta";
        public string OrderAsdDes { get; set; } = "desc";
       
    }
}
