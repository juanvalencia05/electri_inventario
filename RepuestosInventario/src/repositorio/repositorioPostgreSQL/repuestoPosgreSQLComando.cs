using NHibernate.Hql.Ast;
using Npgsql;
using RepuestosInventario.src.dominio;
using System;
using System.Data;
using System.Windows.Forms;

namespace RepuestosInventario.src.repositorio.repositorioPostgreSQL
{
    public class repuestoPosgreSQLComando : repuestoInterfaceComando
    {
        private PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
        public void guardarRepuesto(repuesto repuesto)
        {
            try
            {
                string sqlInsertar = "INSERT INTO repuesto (referencia, nombre, marca, cantidad, precio, costo) " +
                    "VALUES (@referencia, @nombre, @marca, @cantidad, @precio, @costo);";

                using (NpgsqlCommand comando = new NpgsqlCommand(sqlInsertar, this.objetoConexion.establecerConexion()))
                {
                    comando.Parameters.AddWithValue("@referencia", repuesto.Referencia);
                    comando.Parameters.AddWithValue("@nombre", repuesto.Nombre);
                    comando.Parameters.AddWithValue("@marca", repuesto.Marca);
                    comando.Parameters.AddWithValue("@cantidad", repuesto.Cantidad);
                    comando.Parameters.AddWithValue("@precio", repuesto.Precio);
                    comando.Parameters.AddWithValue("@costo", repuesto.Costo);

                    comando.ExecuteNonQuery();
                }
                MessageBox.Show("Se pudo guardar la información");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la información: " + ex.Message);
            }
            finally
            {
                objetoConexion.cerrarConexion();
            }
        }

        public void modificarRepuesto(string referencia, short cantidad)
        {
            try {
                string sqlUpdateCantidad = "UPDATE repuesto SET cantidad=@cantidad WHERE referencia=@referencia;";

                using (NpgsqlCommand comando = new NpgsqlCommand(sqlUpdateCantidad, objetoConexion.establecerConexion()))
                {
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    comando.Parameters.AddWithValue("@referencia", referencia);

                    comando.ExecuteNonQuery();
                }

                MessageBox.Show("Se actualizó la información");
            }
            catch (Exception ) {
                MessageBox.Show("No se pudo actualizar la información");
            }
            finally {
                objetoConexion.cerrarConexion();
            }
        }

        public void modificarRepuestoPrecio(string referencia, double precio, double costo)
        {
            string sqlUpdate = ElejirValorACambiar(precio, costo, referencia);
            try
            {
                NpgsqlCommand comando = new NpgsqlCommand(sqlUpdate, this.objetoConexion.establecerConexion());
                comando.ExecuteNonQuery();
                MessageBox.Show("Se Actualizo la información");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la información: " + ex.Message);
            }
            finally
            {
                objetoConexion.cerrarConexion();
            }
        }
        private string ElejirValorACambiar(double precio, double costo, string referencia)
        {
            string sqlUpdate = "UPDATE  repuesto ";  

            if (precio > 0 && costo > 0)
            {
                sqlUpdate += "SET precio=" + precio + ", costo=" + costo +
                                   " WHERE referencia='" + referencia + "';";
            }
            else if (precio > 0 && costo == 0)
            {
                sqlUpdate += "SET precio=" + precio +
                                    " WHERE referencia='" + referencia + "';";
            }
            else if(costo > 0 && precio == 0)
            {
                sqlUpdate += "SET costo=" + costo +
                                    " WHERE referencia='" + referencia + "';";
            }
            return sqlUpdate;
        }

        public void eliminarRepuesto(string referencia)
        {
            try {
                string sqlDelete = "DELETE FROM repuesto WHERE referencia=@referencia;";

                using (NpgsqlCommand comando = new NpgsqlCommand(sqlDelete, this.objetoConexion.establecerConexion()))
                {
                    comando.Parameters.AddWithValue("@referencia", referencia);

                    comando.ExecuteNonQuery();
                }

                MessageBox.Show("Se eliminó el repuesto");
            }
            catch (Exception) {
                MessageBox.Show("No se pudo eliminar el repuesto");
            }
            finally {
                objetoConexion.cerrarConexion();
            }
        }
    }
}
