using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RepuestosInventario.src.trasnversal
{
    public class EdicionData
    {
        public void columnasSoloLectura(DataGridView tablaRespuestos)
        {
            tablaRespuestos.Columns["id"].Visible = false;

            tablaRespuestos.Columns["referencia"].ReadOnly = true;
            tablaRespuestos.Columns["nombre"].ReadOnly = true;
            tablaRespuestos.Columns["marca"].ReadOnly = true;
            tablaRespuestos.Columns["cantidad"].ReadOnly = true;
            tablaRespuestos.Columns["precio"].ReadOnly = true;
            tablaRespuestos.Columns["costo"].ReadOnly = true;
        }
        public void aplicarFormatoNumerico(DataGridView tablaRespuestos)
        {
            tablaRespuestos.Columns["precio"].DefaultCellStyle.Format = "#,#";
            tablaRespuestos.Columns["costo"].DefaultCellStyle.Format = "#,#";
        }

        public void asignarTituloColumnasDimencion(DataGridView tablaRespuestos)
        {
            tablaRespuestos.Columns["referencia"].HeaderText = "Referencia";
            tablaRespuestos.Columns["nombre"].HeaderText = "Nombre";
            tablaRespuestos.Columns["marca"].HeaderText = "Marca";
            tablaRespuestos.Columns["cantidad"].HeaderText = "Cantidad";
            tablaRespuestos.Columns["precio"].HeaderText = "Precio";
            tablaRespuestos.Columns["costo"].HeaderText = "Costo";

        }
    }
}
