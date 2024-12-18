using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_bancario.Domain.Filters
{
    public class ClienteFilter
    {
        public int Page { get; set; }
        public int IdTipoCliente { get; set; } = 0;
    }
}
