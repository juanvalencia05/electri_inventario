using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using RepuestosInventario.src.dominio;
using System.Collections.Generic;
using System;
using System.IO;

namespace RepuestosInventario.src.trasnversal
{
    public class Imprimir
    {
        private PrintDocument printDocument = new PrintDocument();
        private List<repuestoVenta> repuestos = new List<repuestoVenta>();
        private string nombreEmpresa = "Electripareja";
        private string nitEmpresa = "";
        private string telefonoEmpresa = "300 1527391";
        private DateTime fechaActual;
        private string formaPago;
        private double totalPago;
        private string mensajeAgradecimiento= "Muchas gracias por su compra";
        private Image logoEmpresa;

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
            Font font = new Font("Arial", 12);
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;

            // Imprimir logo de la empresa
            if (logoEmpresa != null)
            {
                graphics.DrawImage(logoEmpresa, new Rectangle(startX, startY, 100, 100));
                startX += 110; // Ajustar la posición para el texto
            }

            // Imprimir nombre de la empresa
            graphics.DrawString(nombreEmpresa, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Imprimir NIT de la empresa
            graphics.DrawString("NIT: " + nitEmpresa, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Imprimir número telefónico de la empresa
            graphics.DrawString("Celular: " + telefonoEmpresa, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Imprimir fecha actual
            graphics.DrawString("Fecha: " + fechaActual.ToString("dd/MM/yyyy"), font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            string encabezados = "Ref\tNombre\tCantidad\tPrecio";
            graphics.DrawString(encabezados, font, Brushes.Black, startX, startY + (int)fontHeight + 5);
            startY += (int)fontHeight + 5;

            // Imprimir lista de productos
            foreach (var producto in repuestos)
            {
                string lineaProducto = $"{producto.repuesto.Referencia}\t{producto.repuesto.Nombre}\t{producto.cantidad}\t{producto.repuesto.Precio.ToString("C")}";
                graphics.DrawString(lineaProducto, font, Brushes.Black, startX, startY);
                startY += (int)fontHeight + 5;
            }

            // Imprimir total a pagar
            graphics.DrawString("Total a pagar: " + totalPago.ToString("C"), font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Imprimir forma de pago
            graphics.DrawString("Forma de pago: " + formaPago, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Imprimir mensaje de agradecimiento
            graphics.DrawString(mensajeAgradecimiento, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

        }
    }
}
