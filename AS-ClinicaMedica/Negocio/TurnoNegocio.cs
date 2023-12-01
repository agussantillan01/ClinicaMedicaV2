using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TurnoNegocio
    {
        public void Agregar(Turno tur)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarTurno");
                datos.setearParametro("@IdPaciente", tur.paciente.IdPaciente);
                datos.setearParametro("@IdMedico", tur.medico.Id);
                datos.setearParametro("@IdEspecialidad", tur.especialidad.Id);
                datos.setearParametro("@Fecha", tur.fecha);
                datos.setearParametro("@Horario", tur.hora);
                datos.setearParametro("@IdEstado", tur.estado.Id);
                datos.setearParametro("@Observacion", tur.Observacion);

                datos.ejectutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Actualizar(Turno tur)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_ActualizarTurno");
                datos.setearParametro("@Id", tur.Id);
                datos.setearParametro("@IdPaciente", tur.paciente.IdPaciente);
                datos.setearParametro("@IdMedico", tur.medico.Id);
                datos.setearParametro("@IdEspecialidad", tur.especialidad.Id);
                datos.setearParametro("@Fecha", tur.fecha);
                datos.setearParametro("@Horario", tur.hora);
                datos.setearParametro("@IdEstado", tur.estado.Id);
                datos.setearParametro("@Observacion", tur.Observacion);

                datos.ejectutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Dominio.Turno> Listar()
        {
            List<Turno> lista = new List<Turno>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT t.ID as idturno,t.FECHA AS FECHATURNO, M.ID AS IDMEDICO, P.ID AS IDPACIENTE,Espe.ID AS IDESPECIALIDAD,t.HORARIO as horarioturno, T.IDESTADO as IDESTADO, T.OBSERVACION AS OBSERVACION FROM TURNOS t inner join pacientes p on p.ID = t.IDPACIENTE Inner join MEDICOS M on M.ID = t.IDMEDICO Inner join ESTADOS E On E.ID = t.IDESTADO Inner join ESPECIALIDADES Espe On Espe.ID = t.IDESPECIALIDAD";
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    PacienteNegocio pacNeg = new PacienteNegocio();
                    MedicoNegocio medNeg = new MedicoNegocio();
                    EspecialidadNegocio espeNeg = new EspecialidadNegocio();
                    EstadoNegocio estadoNeg = new EstadoNegocio();
                    Turno turno = new Turno()
                    {
                        Id = (int)lector["idturno"],
                        paciente = pacNeg.ListarPacientes().FirstOrDefault(x => x.IdPaciente == (int)lector["IDPACIENTE"]),
                        medico = medNeg.Listar().FirstOrDefault(x => x.Id == (int)lector["IDMEDICO"]),
                        especialidad = espeNeg.Listar().FirstOrDefault(x => x.Id == (int)lector["IDESPECIALIDAD"]),
                        estado = estadoNeg.listar().FirstOrDefault(x => x.Id == (int)lector["IDESTADO"]),
                        hora = (string)lector["horarioturno"],
                        fecha = (DateTime)lector["FECHATURNO"],
                        Observacion = (string)lector["OBSERVACION"]

                    };



                    lista.Add(turno);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }

        public void Eliminar(Dominio.Turno turno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_EliminarTurno");
                datos.setearParametro("@ID", turno.Id);
                datos.ejectutarAccion();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


    }
}
