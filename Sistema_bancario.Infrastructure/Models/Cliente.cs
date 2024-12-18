using System;
using System.Collections.Generic;

namespace Sistema_bancario.Infrastructure.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? NumeroIdentificacionCliente { get; set; }

    public string? NombreCliente { get; set; }

    public string? Telefono { get; set; }

    public int? IdTipoCliente { get; set; }

    public string? RepresentanteLegalIdentificacion { get; set; }

    public string? RepresentanteLegalNombre { get; set; }

    public string? RepresentanteLegalTelefono { get; set; }

    public virtual ICollection<Cuentum> Cuenta { get; set; } = new List<Cuentum>();

    public virtual TipoCliente? IdTipoClienteNavigation { get; set; }
}
