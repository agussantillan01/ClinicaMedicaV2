using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Turno
    {
        public int Id { get; set; }
        public Paciente paciente { get; set; }
        public Medico medico { get; set; }
        public Especialidad especialidad { get; set; }
        public DateTime fecha { get; set; }
        public string hora { get; set; }
        public Estado estado { get; set; }
        public string Observacion { get; set; }
    }

}
