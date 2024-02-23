using RepuestosInventario.src.dominio;
using RepuestosInventario.src.repositorio.repositorioPostgreSQL;
using RepuestosInventario.src.trasnversal;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace RepuestosInventario
{
    public partial class FormInventario : Form
    {
        private repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
        private repuestoPosgreSQLComando repuestosComando = new repuestoPosgreSQLComando();
        List<repuestoVenta> inventario = new List<repuestoVenta>();
        private DataGridViewButtonColumn eliminarButtonColumn;
        Imprimir printer;

        public FormInventario()
        {
            InitializeComponent();
            panelBusqueda.Visible = false;
            ocultargroupbox();
        }

        private void ocultargroupbox()
        {
            groupBoxRegistro.Visible = true;
            groupBoxLista.Visible = false;
            groupBoxVenta.Visible = false;
            groupBoxBuscarReferencia.Visible = false;
            groupBoxBuscarNombre.Visible = false;
            groupBoxActualizar.Visible = false;
            groupBusquedaMarca.Visible = false;
            groupBoxEliminar.Visible = false;
            groupBoxImprecion.Visible = false;
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
            if (groupBoxImprecion.Visible == true)
                groupBoxImprecion.Visible = false;
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
        private void vaciarTables()
        {

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
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor diligencie todos los campos del formulario");
                }
            }

        }

        private void retiroIngreso_Click(object sender, EventArgs e)
        {
            short cant = 0;
            try {
                if (referenciaModificar.Text != "" && cantidadModificar.Text != "")
                {
                    repuesto repuesto = this.repuestosConsulta.mostrarRepuestosPorReferenciaParaModificar(referenciaModificar.Text);

                    if (ingresoCheck.Checked && repuesto != null)
                    {
                        cant = (short)(repuesto.Cantidad + short.Parse(cantidadModificar.Text));
                        this.retiroIngresoCantidad(referenciaModificar.Text, cant);

                    }
                    else if (ventaCheck.Checked && repuesto != null)
                    {
                        cant = (short)(repuesto.Cantidad - short.Parse(cantidadModificar.Text));
                        if (cant >= 0)
                        {
                            this.retiroIngresoCantidad(referenciaModificar.Text, cant);
                        }
                        else
                        {
                            MessageBox.Show("No tiene cantidad suficientes de esta referencia", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (!ingresoCheck.Checked && !ventaCheck.Checked)
                    {
                        MessageBox.Show("No ha seleccionado ninguna operación");
                    }
                    this.repuestosConsulta.mostrarRepuestosPorReferencia(tablaRetiro, referenciaModificar.Text);
                    cantidadModificar.Text = "";
                    referenciaModificar.Text = "";
                }
                else 
                {
                    MessageBox.Show("Deba ingresar una Referencia ó cantidad");
                }
            }catch (Exception)
            {
                MessageBox.Show("No se pudo realizar la operación");
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
            if(referenciaModificarPrecio.Text != "")
            {
                if (precioModificar.Text != "0" || costoModificar.Text != "0")
                {
                    this.repuestosComando.modificarRepuestoPrecio(referenciaModificarPrecio.Text, double.Parse(precioModificar.Text), double.Parse(costoModificar.Text));
                    referenciaModificarPrecio.Text = "";
                    costoModificar.Text = "";
                    precioModificar.Text = "";
                }
                else
                {
                    MessageBox.Show("No ha digitado ninguna información");
                }

            } else
            {
                MessageBox.Show("No ha digitado la referencia");
            }

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

        private void bucarImprimirBT_Click(object sender, EventArgs e)
        {
            repuesto repuestoAgregado = this.repuestosConsulta.mostrarRepuestosPorReferenciaParaModificar(buscarImprimirTB.Text);
       
            if (repuestoAgregado != null 
                && this.inventario.Find(repuesto => repuesto.repuesto.Referencia == repuestoAgregado.Referencia) == null)
            {
                repuestoVenta repuesto = new repuestoVenta(repuestoAgregado, 1);
                this.inventario.Add(repuesto);
                if (!dataGridViewImprimir.Columns.Contains("Nombre"))
                {
                    dataGridViewImprimir.Columns.Add("Referencia", "Referencia");
                    dataGridViewImprimir.Columns.Add("Nombre", "Nombre");
                    dataGridViewImprimir.Columns.Add("Precio", "Precio");
                    dataGridViewImprimir.Columns.Add("Cantidad disponible", "Cantidad disponible");
                    dataGridViewImprimir.Columns.Add("Cantidad venta", "Cantidad venta");
                }
                dataGridViewImprimir.DataSource = null;
                this.agregarFilas();

                if (!dataGridViewImprimir.Columns.Contains("Eliminar"))
                {
                    eliminarButtonColumn = new DataGridViewButtonColumn();
                    eliminarButtonColumn.HeaderText = "Eliminar";
                    eliminarButtonColumn.Text = "Eliminar";
                    eliminarButtonColumn.Name = "Eliminar";
                    eliminarButtonColumn.UseColumnTextForButtonValue = true;

                    dataGridViewImprimir.Columns.Add(eliminarButtonColumn);
                }
                this.agregarColumnaAlFinal();
            }else if (repuestoAgregado == null) {
                MessageBox.Show("No hay repuesto con esa referencia");

            } else
            {
                MessageBox.Show("Ya agrego este repuesto");
            }
        }

        private void dataGridViewImprimir_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int posicion = e.RowIndex;
                if (dataGridViewImprimir.Columns.Contains("Eliminar"))
                {
                    DataGridViewColumn columnaEliminar = dataGridViewImprimir.Columns["Eliminar"];
                    if (columnaEliminar != null && e.ColumnIndex == columnaEliminar.Index)
                    {
                        this.eliminarFilas(posicion);

                        this.agregarFilas();

                        this.agregarColumnaAlFinal();
                    }
                } else {
                    this.eliminarFilas(posicion);

                    this.agregarFilas();

                    this.agregarColumnaAlFinal();
                }
            }
        }
        void agregarColumnaAlFinal()
        {
            if (eliminarButtonColumn != null)
            {
                eliminarButtonColumn.DisplayIndex = dataGridViewImprimir.ColumnCount - 1;
            }
        }
        
        void eliminarFilas(int posicion)
        {
            if (posicion < this.inventario.Count)
            {
                this.inventario.RemoveAt(posicion);
            }
        }

        void agregarFilas()
        {
            dataGridViewImprimir.Rows.Clear();
            foreach (var repuestoIm in this.inventario)
            {
                dataGridViewImprimir.Rows.Add(repuestoIm.Repuesto.Referencia, repuestoIm.Repuesto.Nombre, repuestoIm.Repuesto.Precio, repuestoIm.Repuesto.Cantidad, repuestoIm.cantidad);
            }
        }

        private void dataGridViewImprimir_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < inventario.Count)
            {
                // Obtener el nuevo valor de cantidad desde la celda editada
                int nuevaCantidad;
                if (int.TryParse(dataGridViewImprimir.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out nuevaCantidad))
                {
                    inventario[e.RowIndex] = new repuestoVenta(inventario[e.RowIndex].repuesto, nuevaCantidad);
                }
                else
                {
                    MessageBox.Show("Cantidad no válida");
                }
            }
        }

        private void factura_Click(object sender, EventArgs e)
        {
            ocultarGroup();
            ocultarSubMenu();
            groupBoxImprecion.Visible = true;
        }

        private void imprimirBT_Click(object sender, EventArgs e)
        {
            bool cantidadCorrecta = true;
            double totalPagar = 0;
            string formaDePago;

            if (this.inventario.Count > 0)
            {
                foreach (var repuestoIm in this.inventario)
                {
                    if(repuestoIm.Repuesto.Cantidad < repuestoIm.cantidad)
                    {
                        cantidadCorrecta = false;
                        MessageBox.Show("El repuesto " + repuestoIm.Repuesto.Nombre + "no tiene la cantidad suficiente");
                    }
                    totalPagar += repuestoIm.Repuesto.Precio * repuestoIm.cantidad;
                }
                formaDePago = formaPago.SelectedItem as String;
                if (formaDePago == null)
                {
                    MessageBox.Show("Por favor seleccione la forma de pago");
                } else
                {
                    if (cantidadCorrecta)
                    {
                        foreach (var repuestoIm in this.inventario)
                        {
                            this.repuestosComando.modificarRepuesto(repuestoIm.Repuesto.Referencia, (short)(repuestoIm.Repuesto.Cantidad - repuestoIm.Cantidad));
                        }
                        dataGridViewImprimir.DataSource = null;
                        buscarImprimirTB.Text = "";
                        Imprimir printer = new Imprimir(this.inventario, totalPagar, formaDePago);
                        printer.Print();
                        this.inventario.Clear();
                    }
                }
            } else
            {
                MessageBox.Show("No hay repuesto seleccionados");
            }

        }

        private void actualizarRepuesto_Click(object sender, EventArgs e)
        {
            if(referenciaModificarPrecio.Text != "")
            { 
                if(referenciaNueva.Text != "" || nombreNuevo.Text != "" || marcaNuevo.Text != "")
                {
                    this.repuestosComando.actualizarRepuesto(referenciaModificarPrecio.Text, referenciaNueva.Text, nombreNuevo.Text, marcaNuevo.Text);
                    referenciaModificarPrecio.Text = "";
                    referenciaNueva.Text = "";
                    nombreNuevo.Text = "";
                    marcaNuevo.Text = "";
                } else
                {
                    MessageBox.Show("No ha digitado ninguna información");
                }
            }
            else
            {
                MessageBox.Show("No ha digitado ninguna referencia");
            }

        }
    }
}
