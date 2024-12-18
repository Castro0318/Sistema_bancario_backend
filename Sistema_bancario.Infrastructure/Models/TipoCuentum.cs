using System;
using System.Collections.Generic;

namespace Sistema_bancario.Infrastructure.Models;

public partial class TipoCuentum
{
    public int IdTipoCuenta { get; set; }

    public string? NombreTipoCuenta { get; set; }

    public virtual ICollection<Cuentum> Cuenta { get; set; } = new List<Cuentum>();
}
