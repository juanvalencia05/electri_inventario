using RepuestosInventario.src.dominio;
using RepuestosInventario.src.repositorio.repositorioPostgreSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RepuestosInventario
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void inicio_Click(object sender, EventArgs e)
        {
            repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
            log log;
            log = repuestosConsulta.inicioSesion(usuario.Text,contrasena.Text);


            if(log != null) 
            { 
                FormInventario inventario = new FormInventario();
                inventario.Show();
                this.Hide();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void usuario_Enter(object sender, EventArgs e)
        {
            if (usuario.Text == "USUARIO")
            {
                usuario.Text = "";
                usuario.ForeColor = Color.Black;
            }
        }

        private void usuario_Leave(object sender, EventArgs e)
        {
            if (usuario.Text == "")
            {
                usuario.Text = "USUARIO";
                usuario.ForeColor = Color.DimGray;
            }
        }

        private void contrasena_Enter(object sender, EventArgs e)
        {
            if (contrasena.Text == "CONTRASEÑA")
            {
                contrasena.Text = "";
                contrasena.ForeColor = Color.Black;
                contrasena.UseSystemPasswordChar= true;
            }
        }

        private void contrasena_Leave(object sender, EventArgs e)
        {
            if (contrasena.Text == "")
            {
                contrasena.Text = "CONTRASEÑA";
                contrasena.ForeColor = Color.DimGray;
                contrasena.UseSystemPasswordChar = false;
            }
        }
    }
}
