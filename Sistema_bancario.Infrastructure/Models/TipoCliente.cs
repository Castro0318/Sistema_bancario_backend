using System;
using System.Collections.Generic;

namespace Sistema_bancario.Infrastructure.Models;

public partial class TipoCliente
{
    public int IdTipoCliente { get; set; }

    public string? NombreTipoCliente { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
