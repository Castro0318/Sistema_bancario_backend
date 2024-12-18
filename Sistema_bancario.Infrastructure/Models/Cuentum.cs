using System;
using System.Collections.Generic;

namespace Sistema_bancario.Infrastructure.Models;

public partial class Cuentum
{
    public int IdCuenta { get; set; }

    public int? IdTipoCuenta { get; set; }

    public int? IdCliente { get; set; }

    public int? IdEstadoCuenta { get; set; }

    public double? Saldo { get; set; }

    public double? InteresCuenta { get; set; }
    public virtual ICollection<HistorialCuentum> HistorialCuenta { get; set; } = new List<HistorialCuentum>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual EstadoCuentum? IdEstadoCuentaNavigation { get; set; }

    public virtual TipoCuentum? IdTipoCuentaNavigation { get; set; }
}
