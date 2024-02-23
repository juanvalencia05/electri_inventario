﻿using NHibernate.Hql.Ast;
using Npgsql;
using RepuestosInventario.src.dominio;
using System;
using System.Data;
using System.Windows.Forms;

namespace RepuestosInventario.src.repositorio.repositorioPostgreSQL
{
    public class repuestoPosgreSQLComando : repuestoInterfaceComando
    {
        public void guardarRepuesto(repuesto repuesto)
        {
            PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
            try
            {
               
                string sqlInsertar = "INSERT INTO repuesto (referencia, nombre, marca, cantidad, precio, costo) " +
                    "VALUES (@referencia, @nombre, @marca, @cantidad, @precio, @costo);";

                using (NpgsqlCommand comando = new NpgsqlCommand(sqlInsertar, objetoConexion.establecerConexion()))
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
            PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
            try {
                string sqlUpdateCantidad = "UPDATE repuesto SET cantidad=@cantidad WHERE referencia=@referencia;";

                using (NpgsqlCommand comando = new NpgsqlCommand(sqlUpdateCantidad, objetoConexion.establecerConexion()))
                {
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    comando.Parameters.AddWithValue("@referencia", referencia);

                    comando.ExecuteNonQuery();
                }
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
            PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();

            string sqlUpdate = ElejirValorACambiar(precio, costo, referencia);
            try
            {
                NpgsqlCommand comando = new NpgsqlCommand(sqlUpdate, objetoConexion.establecerConexion());
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
        public void actualizarRepuesto(string referencia, string referenciaActualizada, string nombre, string marca)
        {
            PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();

            string sqlUpdate= ElejirCampoActualizar(referencia, referenciaActualizada, nombre, marca);
            try
            {
                NpgsqlCommand comando = new NpgsqlCommand(sqlUpdate, objetoConexion.establecerConexion());
                comando.ExecuteNonQuery();
                MessageBox.Show("Se Actualizo la información");
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar la información");
            }
            finally
            {
                objetoConexion.cerrarConexion();
            }
        }

        private string ElejirCampoActualizar(string referencia, string referenciaActualizada, string nombre, string marca)
        {
            string sqlUpdate = "UPDATE  repuesto SET ";

            if (referenciaActualizada != "")
            {
                sqlUpdate += "referencia= '" + referenciaActualizada + "'";
            }
            if (nombre != "")
            { 
                if(referenciaActualizada != "")
                {
                    sqlUpdate += ", nombre= '" + nombre + "'";
                } else
                {
                    sqlUpdate += "nombre= '" + nombre + "'";
                }
                
            }
            if (marca != "")
            {
                if (nombre != "")
                {
                    sqlUpdate += ", marca= '" + marca + "'";
                }
                else
                {
                    sqlUpdate += "marca= '" + marca + "'";
                }
            }
            sqlUpdate += " WHERE referencia='" + referencia + "';";

            return sqlUpdate;
        }

        public void eliminarRepuesto(string referencia)
        {
            PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();

            try {
                string sqlDelete = "DELETE FROM repuesto WHERE referencia=@referencia;";

                using (NpgsqlCommand comando = new NpgsqlCommand(sqlDelete, objetoConexion.establecerConexion()))
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
