using System;
using System.Windows.Forms;

namespace RepuestosInventario.src.dominio
{
    public class repuesto
    {
        private string referencia;
        private string nombre;
        private string marca;
        private short cantidad;
        private double precio;
        private double costo;

        public static repuesto build(string referencia, string nombre, string marca, short cantidad, double precio, double costo)
        {
            return new repuesto(referencia,nombre,marca,cantidad,precio, costo);
        }

        public repuesto(string referencia, string nombre, string marca, short cantidad, double precio, double costo)
        {
            this.referencia = ValidarCampoVacio(referencia, "No se digito la referencia ");
            this.nombre = ValidarCampoVacio(nombre, "No se digito el nombre ");
            this.marca = ValidarCampoVacio(marca, "No se digita la marca ");
            this.cantidad = ((short)ValidarCampoNumerico(Convert.ToInt32(cantidad), "La cantidad no puede ser menor a 0 "));
            this.precio = ValidarCampoNumerico(precio, "El precio no puede ser menor a 0 ");
            this.costo = ValidarCampoNumerico(costo, "El costo no puede ser menor a 0 ");
        }

        private string ValidarCampoVacio(string valor, string mensaje)
        {
            if (valor.Equals("")) {
                MessageBox.Show(mensaje);
                throw new Exception(mensaje);
            }
            else {
                return valor;
            }
        }
        private double ValidarCampoNumerico(double valor, string mensaje)
        {
            if (valor < 0) {
                MessageBox.Show(mensaje);
                throw new Exception(mensaje);
            }
            else {
                return valor;
            }
        }
        public string Referencia
        {
            get { return this.referencia; }
        }
        public string Marca
        {
            get { return this.marca; }
        }
        public string Nombre
        {
            get { return this.nombre; }
        }
        public short Cantidad
        {
            get { return this.cantidad; }
        }
        public double Precio
        {
            get { return this.precio; }   
        }
        public double Costo
        {
            get { return this.costo; }
        }


    }
}
