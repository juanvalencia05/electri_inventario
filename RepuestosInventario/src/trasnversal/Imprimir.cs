using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using RepuestosInventario.src.dominio;
using System.Collections.Generic;
using System;

namespace RepuestosInventario.src.trasnversal
{
    public class Imprimir
    {
        private PrintDocument printDocument = new PrintDocument();
        private List<repuestoVenta> repuestos = new List<repuestoVenta>();
        private string nombreEmpresa = "Electripareja";
        private string nitEmpresa = "39448792";
        private string telefonoEmpresa = "300 1527391";
        private DateTime fechaActual = DateTime.Now;
        private string formaPago;
        private double totalPago;
        private string mensajeAgradecimiento= "Muchas gracias por su compra";
        private Image logoEmpresa = Properties.Resources.ElectriparejaImagen;

        public Imprimir(List<repuestoVenta> repuestos, double totalPagar, string formaPago)
        {
            this.repuestos = repuestos;
            this.totalPago = totalPagar;
            this.formaPago = formaPago;
            printDocument.PrintPage += PrintPageHandler;
        }

        public void Print()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 8);
            float fontHeight = font.GetHeight();
            int paperWidth = e.PageBounds.Width;
            int startX = (int)(paperWidth - 100) / 2; // Centrar el logo
            int startY = 10;
            int lineHeight = 2; // Altura de la línea punteada
            int lineSpacing = 5;
            Pen pen = new Pen(Brushes.Black, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            if (logoEmpresa != null)
            {
                graphics.DrawImage(logoEmpresa, new Rectangle(startX, startY, 100, 100));
                startY += 110;
            }
            int nombreEmpresaWidth = (int)graphics.MeasureString(nombreEmpresa, font).Width;
            int nitEmpresaWidth = (int)graphics.MeasureString("NIT: " + nitEmpresa, font).Width;
            int telefonoEmpresaWidth = (int)graphics.MeasureString("Celular: " + telefonoEmpresa, font).Width;
            int fechaActualWidth = (int)graphics.MeasureString("Fecha: " + fechaActual.ToString("dd/MM/yyyy HH:mm"), font).Width;

            // Centrar el nombre de la empresa
            startX = (paperWidth - nombreEmpresaWidth) / 2;
            graphics.DrawString(nombreEmpresa, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Centrar el NIT de la empresa
            startX = (paperWidth - nitEmpresaWidth) / 2;
            graphics.DrawString("NIT: " + nitEmpresa, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Centrar el número telefónico de la empresa
            startX = (paperWidth - telefonoEmpresaWidth) / 2;
            graphics.DrawString("Celular: " + telefonoEmpresa, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            graphics.DrawString("Fecha: " + fechaActual.ToString("dd/MM/yyyy"), font, Brushes.Black, 0, startY);

            startX = (paperWidth - fechaActualWidth - 10) / 2;
            graphics.DrawString("Hora: " + fechaActual.ToString("HH:mm"), font, Brushes.Black, startX + 100, startY);
            startY += (int)fontHeight + 5;

            // Dibujar línea punteada debajo de cada producto
            graphics.DrawLine(pen, 0, startY, paperWidth, startY);
            startY += lineHeight;

            // Espacio debajo de la línea punteada
            startY += lineSpacing;

            string encabezados = "Nom\tCant";
            graphics.DrawString(encabezados, font, Brushes.Black, 0, startY);

            string encabezado2 = "Precio";
            graphics.DrawString(encabezado2, font, Brushes.Black, startX + 50, startY);

            string encabezado3 = "Total";
            graphics.DrawString(encabezado3, font, Brushes.Black, paperWidth - 70, startY);
            startY += (int)fontHeight + 10;

            // Imprimir lista de productos
            foreach (var producto in repuestos)
            {
                startY += lineSpacing;

                string nombreProducto = $"{producto.repuesto.Nombre}";
                graphics.DrawString(nombreProducto, font, Brushes.Black, 0, startY); // Imprimir nombre del producto desde el borde izquierdo
                startY += (int)fontHeight + 2;

                string cantidadProducto = $"{producto.cantidad}";
                graphics.DrawString(cantidadProducto, font, Brushes.Black, 50, startY); // Imprimir nombre del producto desde el borde izquierdo

                string precioProducto = (producto.repuesto.Precio).ToString("C");
                graphics.DrawString(precioProducto, font, Brushes.Black, startX + 50, startY); // Imprimir nombre del producto desde el borde izquierdo

                string totalProducto = (producto.repuesto.Precio * producto.cantidad).ToString("C");
                graphics.DrawString(totalProducto, font, Brushes.Black, paperWidth - 80, startY); // Imprimir nombre del producto desde el borde izquierdo
                startY += (int)fontHeight + 2;

                graphics.DrawLine(pen, 0, startY, paperWidth, startY);
                startY += lineHeight;

                startY += lineSpacing;
            }

            graphics.DrawString("Total a pagar: ", font, Brushes.Black, 0, startY);

            // Imprimir el valor del total a pagar alineado a la derecha
            graphics.DrawString(totalPago.ToString("C"), font, Brushes.Black, paperWidth - 80, startY);
            startY += (int)fontHeight + 5;

            // Imprimir forma de pago
            graphics.DrawString("Forma de pago: " + formaPago, font, Brushes.Black, 0, startY);
            startY += (int)fontHeight + 5;

            // Dibujar línea punteada debajo de cada producto
            graphics.DrawLine(pen, 0, startY, paperWidth, startY);
            startY += lineHeight;

            // Espacio debajo de la línea punteada
            startY += lineSpacing;

            // Imprimir forma de pago
            graphics.DrawString("Cliente: ", font, Brushes.Black, 0, startY);
            startY += (int)fontHeight + 5;

            graphics.DrawString("CC/NIT: ", font, Brushes.Black, 0, startY);
            startY += (int)fontHeight + 5;

            // Dibujar línea punteada debajo de cada producto
            graphics.DrawLine(pen, 0, startY, paperWidth, startY);
            startY += lineHeight;

            // Imprimir mensaje de agradecimiento
            graphics.DrawString(mensajeAgradecimiento, font, Brushes.Black, 60, startY);

        }
    }
}
