using Npgsql;
using RepuestosInventario.src.dominio;
using RepuestosInventario.src.trasnversal;
using System;
using System.Data;
using System.Windows.Forms;

namespace RepuestosInventario.src.repositorio.repositorioPostgreSQL
{
    public class repuestoPostgreSQLConsulta : repuestoInterfaceConsulta
    {
        EdicionData edicion = new EdicionData();

        public log inicioSesion(string usuario, string contrasena)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                log login;

                string sqlConsulta = "select * from usuario WHERE usuario='" + usuario + "'and contrasena='" + contrasena + "';";

                NpgsqlCommand comando = new NpgsqlCommand(sqlConsulta, objetoConexion.establecerConexion());
                NpgsqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {                 
                    login =  log.build(reader["usuario"].ToString(), reader["contrasena"].ToString());
                }
                else
                {
                    MessageBox.Show("EL usuario o la contraseña son incorrectos ");
                    login = null;
                }
                objetoConexion.cerrarConexion();
                return login;
            }
            catch
            {
                MessageBox.Show("No se puedo Iniciar sesion");
                return null;
            }
        }

        public void mostrarRepuestos(DataGridView tablaRespuestos)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();

                string sqlConsulta = "select *from repuesto";

                tablaRespuestos.DataSource = null;
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlConsulta, objetoConexion.establecerConexion());

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                tablaRespuestos.DataSource = dataTable;

                edicion.asignarTituloColumnasDimencion(tablaRespuestos);
                edicion.aplicarFormatoNumerico(tablaRespuestos);
                edicion.columnasSoloLectura(tablaRespuestos);

                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo mostrar la información");
            }
        }

        public void mostrarRepuestosPorNombre(DataGridView tablaRespuestos, string nombre)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                string sqlConsulta = "select * from repuesto WHERE nombre LIKE'%" + nombre + "%';";
                tablaRespuestos.DataSource = null;

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlConsulta, objetoConexion.establecerConexion());
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                tablaRespuestos.DataSource = dataTable;
                edicion.asignarTituloColumnasDimencion(tablaRespuestos);
                edicion.aplicarFormatoNumerico(tablaRespuestos);
                edicion.columnasSoloLectura(tablaRespuestos);

                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo mostrar la información");
            }
        }

        public void mostrarRepuestosPorReferencia(DataGridView tablaRespuestos, string referencia)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                string sqlConsulta = "select * from repuesto WHERE referencia LIKE '%" + referencia + "%';";
                tablaRespuestos.DataSource = null;

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlConsulta, objetoConexion.establecerConexion());
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                tablaRespuestos.DataSource = dataTable;

                edicion.asignarTituloColumnasDimencion(tablaRespuestos);
                edicion.aplicarFormatoNumerico(tablaRespuestos);
                edicion.columnasSoloLectura(tablaRespuestos);

                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo mostrar la Referencia");
            }
        }
        public void mostrarRepuestosPorMarca(DataGridView tablaRespuestos, string marca)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                string sqlConsulta = "select * from repuesto WHERE marca LIKE '%" + marca + "%';";
                tablaRespuestos.DataSource = null;

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlConsulta, objetoConexion.establecerConexion());
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                tablaRespuestos.DataSource = dataTable;

                edicion.asignarTituloColumnasDimencion(tablaRespuestos);
                edicion.aplicarFormatoNumerico(tablaRespuestos);
                edicion.columnasSoloLectura(tablaRespuestos);

                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo mostrar el producot por esa marca");
            }
        }

        public repuesto mostrarRepuestosPorReferenciaParaModificar(string referencia)
        {
            PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
            try
            {
                using (var conn = objetoConexion.establecerConexion())
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM repuesto WHERE referencia = @referencia", conn))
                    {
                        cmd.Parameters.AddWithValue("referencia", referencia);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return repuesto.build(
                                    reader["referencia"].ToString(),
                                    reader["nombre"].ToString(),
                                    reader["marca"].ToString(),
                                    short.Parse(reader["cantidad"].ToString()),
                                    int.Parse(reader["precio"].ToString()),
                                    int.Parse(reader["costo"].ToString()));
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"No existe este repuesto");
                return null;
            }
        }

        public bool consultaDevuelveInformacion(string referencia)
        {
            PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
            string sqlConsulta = "select * from repuesto WHERE referencia ='" + referencia + "';";
            
            using (var comando = new NpgsqlCommand(sqlConsulta, objetoConexion.establecerConexion()))
            {
                var resultado = comando.ExecuteScalar();
                return resultado != null;
            }
        }
    }
}
