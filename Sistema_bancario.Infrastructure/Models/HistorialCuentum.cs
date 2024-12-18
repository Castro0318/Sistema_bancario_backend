using System;
using System.Collections.Generic;

namespace Sistema_bancario.Infrastructure.Models;

public partial class HistorialCuentum
{
    public int IdHistorialCuenta { get; set; }

    public int? IdCuenta { get; set; }

    public double? SaldoNuevo { get; set; }

    public double? SaldoAnterior { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Cuentum? IdCuentaNavigation { get; set; }
}
