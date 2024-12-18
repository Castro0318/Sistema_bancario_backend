using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_bancario.Infrastructure.Models;

namespace Sistema_bancario.Domain.Entities
{
    public class CuentaEntity
    {
        public int IdCuenta { get; set; }
        public int? IdTipoCuenta { get; set; } //Llave foranea q    ue hace referencia tipo cuenta
        public int? IdCliente { get; set; } //Llave foranea que hace referencia a cliente 
        public int? IdEstadoCuenta { get; set; } //Llave foranea que hace referencia a estado cuenta
        public double? Saldo { get; set; }
        public double? InteresCuenta { get; set; }
        public string? NombreTipoCuenta { get; set; } //Nombre de la llave foranea de tipo cuenta
        public string? NombreCliente {  get; set; } //Nombre de la llave foranea de cliente
        public string? NombreEstadoCuenta { get; set; } //Nombre de la llave foranea de estado cuenta

        public CuentaEntity()
        {
            this.IdCuenta = 0;
            this.IdTipoCuenta = 0;
            this.IdCliente = 0;
            this.IdEstadoCuenta = 0;
            this.Saldo = 0;
            this.InteresCuenta = 0;
            this.NombreTipoCuenta = string.Empty;
            this.NombreCliente = string.Empty;
            this.NombreEstadoCuenta = string.Empty;

            //Aca todos los campos los debe de asignar a 0 o a string.empty segun el campo 
        }

        public CuentaEntity(Cuentum cuenta) // Aca recibi un modelo Cuentum y lo llama cuenta
        {
            //Aca asigna todo lo que trae cuenta de tipo Cuentum(modelo)
            this.IdCuenta = cuenta.IdCuenta;
            this.IdTipoCuenta = cuenta.IdTipoCuenta;
            this.IdCliente = cuenta.IdCliente;
            this.IdEstadoCuenta = cuenta.IdEstadoCuenta;
            this.Saldo = cuenta.Saldo;
            this.InteresCuenta = cuenta.InteresCuenta;

            //Valide que el IdTipoCuentaNavigation no este nulo osea que tenga algo
            if(cuenta.IdTipoCuentaNavigation != null)
            {
                this.NombreTipoCuenta = cuenta.IdTipoCuentaNavigation.NombreTipoCuenta; //Asigna el nombre de la llave foranea con el navigation.NombreTipoCuenta
            }

            //Hace lo mismo con los demas navigation y campos Nombres de las llaves foraneas 
            if(cuenta.IdClienteNavigation != null)
            {
                this.NombreCliente = cuenta.IdClienteNavigation.NombreCliente;
            }
            if(cuenta.IdEstadoCuentaNavigation != null)
            {
                this.NombreEstadoCuenta = cuenta.IdEstadoCuentaNavigation.Nombre;
            }
        }
    }
}
