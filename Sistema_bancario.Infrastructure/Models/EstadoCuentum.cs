using System;
using System.Collections.Generic;

namespace Sistema_bancario.Infrastructure.Models;

public partial class EstadoCuentum
{
    public int IdEstadoCuenta { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Cuentum> Cuenta { get; set; } = new List<Cuentum>();
}
