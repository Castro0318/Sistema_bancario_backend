using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Domain.Entities
{
    public class ClienteEntity
    {
        public int IdCliente { get; set; }

        public string? NumeroIdentificacionCliente { get; set; }

        public string? NombreCliente { get; set; }

        public string? Telefono { get; set; }

        public int? IdTipoCliente { get; set; }

        public string? RepresentanteLegalIdentificacion { get; set; }

        public string? RepresentanteLegalNombre { get; set; }

        public string? RepresentanteLegalTelefono { get; set; }
        public string? NombreTipoCliente { get; set; } // Este es el nombre de la llave foranea 


        public ClienteEntity()
        {
            this.IdCliente = 0;
            this.NumeroIdentificacionCliente = string.Empty;
            this.NombreCliente = string.Empty;
            this.Telefono = string.Empty;
            this.IdTipoCliente = 0;
            this.RepresentanteLegalIdentificacion = string.Empty;
            this.RepresentanteLegalNombre = string.Empty;
            this.RepresentanteLegalTelefono = string.Empty;
            this.NombreTipoCliente = string.Empty;
        }
        public ClienteEntity(Cliente Cliente)
        {
            this.IdCliente = Cliente.IdCliente;
            this.NumeroIdentificacionCliente = Cliente.NumeroIdentificacionCliente;
            this.Telefono = Cliente.Telefono;
            this.NombreCliente = Cliente.NombreCliente;
            this.IdTipoCliente = Cliente.IdTipoCliente;
            this.RepresentanteLegalIdentificacion = Cliente.RepresentanteLegalIdentificacion;
            this.RepresentanteLegalNombre = Cliente.RepresentanteLegalNombre;
            this.RepresentanteLegalTelefono = Cliente.RepresentanteLegalTelefono;
            if (Cliente.IdTipoClienteNavigation != null) //Aqui valida que el nagation no venga vacio 
            {
                this.NombreTipoCliente = Cliente.IdTipoClienteNavigation.NombreTipoCliente; // Aqui asigna el valor de lnavigation al nombre 
            }
        }
    }
}
