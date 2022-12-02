using FluentNHibernate.Cfg.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using System.Windows.Forms;

namespace RepuestosInventario.src.repositorio.repositorioPostgreSQL
{
    public class PostgreSQLConfiguration
    {
        public PostgreSQLConfiguration() { }
        NpgsqlConnection conex = new NpgsqlConnection();

        static string servidor= "localhost";
        static string bd = "repuestosDB";
        static string usuario = "postgres";
        static string password = "password";
        static string puerto = "5432";

        string cadenaConexion = "server=" + servidor + ";"+"port="+puerto+";"+"user id="+usuario+ ";"+
            "Password="+password+";"+ "Database="+bd+";";

        public NpgsqlConnection establecerConexion() 
        {
            try
            {
                conex.ConnectionString= cadenaConexion;
                conex.Open();
            }
            catch(NpgsqlException e) 
            {
                MessageBox.Show("No se conecto" + e.ToString());
            }
            return conex;
        } 
        public void cerrarConexion()
        {
            conex.Close();  
        }    
    }
}
 