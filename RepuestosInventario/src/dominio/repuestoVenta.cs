using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepuestosInventario.src.dominio
{
    public class repuestoVenta
    {
        public repuesto repuesto;
        public int cantidad;

        public repuestoVenta() {}

        public repuestoVenta(repuesto repuesto, int cantidad)
        {
            this.repuesto = repuesto;
            this.cantidad = cantidad;
        }
        public repuesto Repuesto
        {
            get { return repuesto; }
        }

        public int Cantidad
        {
            get { return cantidad; }
        }
    }
}
