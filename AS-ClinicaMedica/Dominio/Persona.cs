using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public Usuario usuario { get; set; }
        public string datos
        {
            get
            {
                return Nombre + " " + Apellido + " - " + DNI;
            }
        }
    }
}
