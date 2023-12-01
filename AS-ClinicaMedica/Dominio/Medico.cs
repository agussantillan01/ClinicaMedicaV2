using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Medico : Persona
    {
        public int Id { get; set; }
        public List<Horario> listHorarios { get; set; }
        public List<Especialidad> listEspecialidades { get; set; }
    }
}
