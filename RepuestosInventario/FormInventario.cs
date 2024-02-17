using RepuestosInventario.src.dominio;
using RepuestosInventario.src.repositorio.repositorioPostgreSQL;
using System;
using System.Windows.Forms;


namespace RepuestosInventario
{
    public partial class FormInventario : Form
    {
        private repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
        private repuestoPosgreSQLComando repuestosComando = new repuestoPosgreSQLComando();

        public FormInventario()
        {
            InitializeComponent();
            panelBusqueda.Visible = false;
            ocultargroupbox();
        }

        private void ocultargroupbox()
        {
            groupBoxRegistro.Visible = false;
            groupBoxLista.Visible = false;
            groupBoxVenta.Visible = false;
            groupBoxBuscarReferencia.Visible = false;
            groupBoxBuscarNombre.Visible = false;
            groupBoxActualizar.Visible = false;
            groupBusquedaMarca.Visible = false;
            groupBoxEliminar.Visible = false;
        }
        private void ocultarGroup()
        {
            if(groupBoxRegistro.Visible == true)
               groupBoxRegistro.Visible = false;
            if (groupBoxLista.Visible == true)
                groupBoxLista.Visible = false;
            if (groupBoxVenta.Visible == true)
                groupBoxVenta.Visible = false;
            if (groupBoxBuscarReferencia.Visible == true)
                groupBoxBuscarReferencia.Visible = false;
            if (groupBoxBuscarNombre.Visible == true)
                groupBoxBuscarNombre.Visible = false;
            if (groupBoxActualizar.Visible == true)
                groupBoxActualizar.Visible = false;
            if (groupBusquedaMarca.Visible == true)
                groupBusquedaMarca.Visible = false;
            if (groupBoxEliminar.Visible == true)
                groupBoxEliminar.Visible = false;
        }
        private void ocultarSubMenu()
        {
            panelBusqueda.Visible = false;
        }
        private void mostrarSubMenu()
        {
            if(panelBusqueda.Visible == false)
            {
                panelBusqueda.Visible = true;
            }
        }
        void formatoMoneda(TextBox textBox) 
        {
            decimal monto;
            if(!textBox.Text.Equals(""))
            {
                monto = Convert.ToDecimal(textBox.Text);
                textBox.Text = monto.ToString("#,#");
            }
        }
        private void guardar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(referencia.Text) || string.IsNullOrEmpty(nombre.Text) || string.IsNullOrEmpty(marca.Text) ||
                string.IsNullOrEmpty(cantidad.Text) || string.IsNullOrEmpty(precio.Text) || string.IsNullOrEmpty(costo.Text))
            {
                MessageBox.Show("Por favor diligencie todos los campos del formulario");
            }
            else if (this.repuestosConsulta.consultaDevuelveInformacion(referencia.Text))
            {
                MessageBox.Show("Ya existe un repuesto con esta referencia");
            }
            else
            {
                try
                {
                    formatoMoneda(precio);
                    formatoMoneda(costo);
                    repuesto repuesto = repuesto.build(referencia.Text, nombre.Text, marca.Text, short.Parse(cantidad.Text), double.Parse(precio.Text), double.Parse(costo.Text));

                    referencia.Text = nombre.Text = marca.Text = cantidad.Text = precio.Text = costo.Text = "";

                    this.repuestosComando.guardarRepuesto(repuesto);
                    this.repuestosConsulta.mostrarRepuestos(listaRepuestos);
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor diligencie todos los campos del formulario");
                }
            }

        }

        private void listaRepuestos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.repuestosConsulta.seleccionarRepuesto(listaRepuestos, referenciaModificar);
        }

        private void retiroIngreso_Click(object sender, EventArgs e)
        {
            short cant = 0;

            if (referenciaModificar.Text != "" && cantidadModificar.Text != "")
            {
                repuesto repuesto = this.repuestosConsulta.mostrarRepuestosPorReferenciaParaModificar(referenciaModificar.Text);
                
                if(ingresoCheck.Checked)
                {
                    cant = (short)(repuesto.Cantidad + short.Parse(cantidadModificar.Text));
                    this.retiroIngresoCantidad(referenciaModificar.Text, cant);

                }
                else if (ventaCheck.Checked)
                {
                    cant = (short)(repuesto.Cantidad - short.Parse(cantidadModificar.Text));
                    if(cant >= 0)
                    {
                        this.retiroIngresoCantidad(referenciaModificar.Text, cant);
                    }
                    else
                    {
                        MessageBox.Show("No tiene cantidad suficientes de esta referencia", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);                      
                    }
                }
                else
                {
                    MessageBox.Show("No a selecionado ninguna operacion");
                }
                this.repuestosConsulta.mostrarRepuestos(listaRepuestos);
                this.repuestosConsulta.mostrarRepuestosPorReferencia(tablaRetiro, referenciaModificar.Text);
            }
            else
            {
                MessageBox.Show("Deba ingresar una Referencia ó cantidad");
            }
        }
        private void buscarReferenciaBT_Click(object sender, EventArgs e)
        {
            this.repuestosConsulta.mostrarRepuestosPorReferencia(tablaBusquedaReferencia, busquedaReferenciaText.Text);

        }
        private void buscarNombreBT_Click(object sender, EventArgs e)
        {
            this.repuestosConsulta.mostrarRepuestosPorNombre(tablaBusquedaNombre, nombreBuscar.Text);
        }

        private void BusquedaMarcaBT_Click(object sender, EventArgs e)
        {
            this.repuestosConsulta.mostrarRepuestosPorMarca(tablaBusquedaMarca, busquedaMarcaText.Text);
        }

        private void retiroIngresoCantidad(string referencia , short cantidad)
        {
            this.repuestosComando.modificarRepuesto(referenciaModificar.Text, cantidad);
        }
        private void ventaCheck_CheckedChanged(object sender, EventArgs e)
        {
            if(ventaCheck.Checked == true)
            {
                ingresoCheck.Checked = false;
            }
        }
        private void ingresoCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ingresoCheck.Checked == true)
            {
                ventaCheck.Checked = false;
            }
        }
        private void modificarValorBT_Click(object sender, EventArgs e)
        {
            if(precioModificar.Text == "")
            {
                precioModificar.Text = "0";
            }
            if (costoModificar.Text == "")
            {
                costoModificar.Text = "0";
            }
            this.repuestosComando.modificarRepuestoPrecio(referenciaModificarPrecio.Text,double.Parse(precioModificar.Text),double.Parse(costoModificar.Text));
            this.repuestosConsulta.mostrarRepuestosPorReferencia(listaRepuestos, referenciaModificarPrecio.Text);
            referenciaModificarPrecio.Text = "";
            costoModificar.Text = "";
            precioModificar.Text = "";

        }
        private void precio_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(precio);
        }
        private void costo_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(costo);
        }
        private void precioModificar_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(precioModificar);
        }
        private void costoModificar_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(costoModificar);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin login = new FormLogin();
            login.Show();
        }

        private void buscarMenu_Click(object sender, EventArgs e)
        {
            mostrarSubMenu();
        }

        private void registrarMenu_Click(object sender, EventArgs e)
        {
            ocultarSubMenu();
            ocultarGroup();
            groupBoxRegistro.Visible = true;
        }

        private void listaProductosMenu_Click(object sender, EventArgs e)
        {
            this.repuestosConsulta.mostrarRepuestos(listaRepuestos);
            ocultarSubMenu();
            ocultarGroup();
            groupBoxLista.Visible = true;
        }
        private void ventaMenu_Click(object sender, EventArgs e)
        {
            ocultarSubMenu();
            ocultarGroup();
            referenciaModificar.Text = "";
            cantidadModificar.Text = "";
            groupBoxVenta.Visible = true;
        }
        private void buscarReferenciaMenu_Click(object sender, EventArgs e)
        {
            ocultarGroup();
            busquedaReferenciaText.Text = "";
            groupBoxBuscarReferencia.Visible = true;
        }

        private void bucarNombreMenu_Click(object sender, EventArgs e)
        {
            ocultarGroup();
            nombreBuscar.Text = "";
            groupBoxBuscarNombre.Visible = true;

        }

        private void buscarMarcaMenu_Click(object sender, EventArgs e)
        {
            ocultarGroup();
            busquedaMarcaText.Text = "";
            groupBusquedaMarca.Visible = true;
        }

        private void actualizarMenu_Click(object sender, EventArgs e)
        {
            ocultarSubMenu();
            ocultarGroup();
            referenciaModificarPrecio.Text = "";
            precioModificar.Text = "";
            costoModificar.Text = "";
            groupBoxActualizar.Visible = true;
        }

        private void eliminarMenu_Click(object sender, EventArgs e)
        {
            ocultarGroup();
            ocultarSubMenu();
            referenciaEliminar.Text = "";
            groupBoxEliminar.Visible = true;
        }

        private void EliminarBT_Click(object sender, EventArgs e)
        {
            this.repuestosComando.eliminarRepuesto(referenciaEliminar.Text);
            referenciaEliminar.Text = "";
        }
    }
}
