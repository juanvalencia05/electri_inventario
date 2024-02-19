using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RepuestosInventario.src.trasnversal
{
    public class Imprimir
    {
        private PrintDocument printDocument = new PrintDocument();
        private string[] lines;
        private int currentLineIndex;

        public Imprimir()
        {
            //this.lines = lines;
            printDocument.PrintPage += PrintPageHandler;
        }

        public void Print()
        {
            currentLineIndex = 0;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 12);
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;

            // Imprimir mensaje de prueba
            string message = "¡Hola! Acabo de imprimir.";
            graphics.DrawString(message, font, Brushes.Black, startX, startY);
            startY += (int)fontHeight + 5;

            // Imprimir detalles de la factura
            /*foreach (var line in lines)
            {
                graphics.DrawString(line, font, Brushes.Black, startX, startY);
                startY += (int)fontHeight + 5;
                currentLineIndex++;
                if (currentLineIndex >= lines.Length)
                {
                    e.HasMorePages = false;
                    break;
                }
                else
                {
                    e.HasMorePages = true;
                }
            }*/
        }
    }
}
