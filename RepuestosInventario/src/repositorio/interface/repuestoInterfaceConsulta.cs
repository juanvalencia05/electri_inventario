using RepuestosInventario.src.dominio;
using System.Windows.Forms;

namespace RepuestosInventario.src.repositorio
{
    public interface repuestoInterfaceConsulta
    {
        void mostrarRepuestos(DataGridView tablaRespuestos);
        repuesto mostrarRepuestosPorReferenciaParaModificar(string referencia);
        void mostrarRepuestosPorReferencia(DataGridView tablaRespuestos, string referencia);

        void mostrarRepuestosPorNombre(DataGridView tablaRespuestos, string nombre);

        void seleccionarRepuesto(DataGridView tablaRespuestos, TextBox referencia);

        log inicioSesion(string usuario, string contrasena);
    }
}
