using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepuestosInventario.src.dominio
{
    public class log
    {
        private string usuario;
        private string contrasena;

        public static log build(string usuario, string contrasena)
        {
            return new log(usuario, contrasena);
        }

        private log(string usuario, string contrasena)
        {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }
    }
}
