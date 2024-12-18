using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Domain.Entities
{
    public class EstadoCuentaEntity
    {
        public int IdEstadoCuenta { get; set; }
        public string? Nombre { get; set; }

        public EstadoCuentaEntity()
        {
            this.IdEstadoCuenta = 0;
            this.Nombre = string.Empty;
        }

        public EstadoCuentaEntity(EstadoCuentum estado)
        {
            this.IdEstadoCuenta= estado.IdEstadoCuenta;
            this.Nombre = estado.Nombre;
        }
    }
}


