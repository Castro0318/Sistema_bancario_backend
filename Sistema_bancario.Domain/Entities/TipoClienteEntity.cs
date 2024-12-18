using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Domain.Entities
{
    public class TipoClienteEntity
    {
        public int IdTipoCliente { get; set; }
        public string? NombreTipoCliente { get; set; }

        public TipoClienteEntity()
        {
            this.IdTipoCliente = 0;
            this.NombreTipoCliente = string.Empty;
        }

        public TipoClienteEntity (TipoCliente TipoCliente)
        {
            this.IdTipoCliente = TipoCliente.IdTipoCliente;
            this.NombreTipoCliente = TipoCliente.NombreTipoCliente;
        }
    }
}
