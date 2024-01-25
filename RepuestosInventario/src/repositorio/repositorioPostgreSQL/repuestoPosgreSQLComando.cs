using NHibernate.Hql.Ast;
using Npgsql;
using RepuestosInventario.src.dominio;
using System.Data;
using System.Windows.Forms;

namespace RepuestosInventario.src.repositorio.repositorioPostgreSQL
{
    public class repuestoPosgreSQLComando : repuestoInterfaceComando
    {
        public void guardarRepuesto(repuesto repuesto)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();

                string sqlInsertar = "INSERT INTO repuesto (referencia, nombre, marca, cantidad, precio, costo) " +
                    "VALUES ('" + repuesto.Referencia + "','" + repuesto.Nombre + "','" + repuesto.Marca + "','" + repuesto.Cantidad + "','" + repuesto.Precio + "','" + repuesto.Costo + "');";

                NpgsqlCommand comando = new NpgsqlCommand(sqlInsertar, objetoConexion.establecerConexion());
 
                NpgsqlDataReader reader = comando.ExecuteReader();
                MessageBox.Show("Se puedo guardo la información");
                while (reader.Read()) { }
                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo registrar la información");
            }
        }

        public void modificarRepuesto(string referencia, short cantidad)
        {
            try {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();

                string sqlUpdateCantidad = "UPDATE  repuesto SET cantidad=" + cantidad +
                    " WHERE referencia='" + referencia + "';";

                NpgsqlCommand comando = new NpgsqlCommand(sqlUpdateCantidad, objetoConexion.establecerConexion());
                NpgsqlDataReader reader = comando.ExecuteReader();
                MessageBox.Show("Se Actualizo la informacion");
                while (reader.Read()){ }
                objetoConexion.cerrarConexion();
            }
            catch {
                MessageBox.Show("No se puedo Actualizo la información");
            }
        }

        public void modificarRepuestoPrecio(string referencia, double precio, double costo)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                string sqlUpdate = ElejirCuantoValoresCambiar(precio,costo, referencia);

                NpgsqlCommand comando = new NpgsqlCommand(sqlUpdate, objetoConexion.establecerConexion());
                NpgsqlDataReader reader = comando.ExecuteReader();
                MessageBox.Show("Se Actualizo la información");
                while (reader.Read())
                {

                }
                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo Actualizar la información");
            }
        }
        private string ElejirCuantoValoresCambiar(double precio, double costo, string referencia)
        {
            string sqlUpdate = "";  

            if (precio > 0 && costo > 0)
            {
                sqlUpdate = "UPDATE  repuesto SET precio=" + precio + ", costo=" + costo +
                                   " WHERE referencia='" + referencia + "';";
            }
            else if (precio > 0 && costo == 0)
            {
                sqlUpdate = "UPDATE  repuesto SET precio=" + precio +
                                    " WHERE referencia='" + referencia + "';";
            }
            else if(costo > 0 && precio ==0)
            {
                sqlUpdate = "UPDATE  repuesto SET costo=" + costo +
                                    " WHERE referencia='" + referencia + "';";
            }
            return sqlUpdate;
        }
    }
}
