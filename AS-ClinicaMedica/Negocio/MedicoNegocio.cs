using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MedicoNegocio
    {
        public List<Dominio.Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select M.ID AS IDMEDICO, M.ID_USUARIODATOPERSONAL AS IDPERSONA, P.NOMBRE AS NOMBRE, P.APELLIDO AS APELLIDO, P.DNI AS DNI, P.EMAIL AS EMAIL, P.SEXO AS SEXO, P.IDUSUARIO AS IDUSER, U.USUARIO AS NOMBREUSUARIO, U.CONTRASENIA AS CLAVE, U.IDTIPOUSER AS IDTIPOUSUARIO, TU.TIPO AS TIPOUSUARIO From MEDICOS M  inner join USUARIOSDATOSPERSONALES P ON P.ID = M.ID_USUARIODATOPERSONAL Inner join USUARIOS U ON U.ID = P.IDUSUARIO INNER JOIN TIPOUSER TU ON TU.ID = U.IDTIPOUSER";
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Medico med = new Medico();

                    med.Id = (int)lector["IDMEDICO"];
                    med.IdPersona = (int)lector["IDPERSONA"];
                    med.Nombre = (string)lector["NOMBRE"];
                    med.Apellido = (string)lector["APELLIDO"];
                    med.DNI = (string)lector["DNI"];
                    med.Email = (string)lector["EMAIL"];
                    med.Sexo = (string)lector["SEXO"];
                    med.usuario = new Usuario(); 
                    med.usuario.Id = (int)lector["IDUSER"];
                    med.usuario.Nombre = (string)lector["NOMBREUSUARIO"];
                    med.usuario.Contrasenia = (string)lector["CLAVE"];
                    med.usuario.perfil = new TipoUsuario();
                    med.usuario.perfil.Id = (int)lector["IDTIPOUSUARIO"];
                    med.usuario.perfil.Tipo = (string)lector["TIPOUSUARIO"];

                    med.listHorarios = this.ListarHorariosXMedico(med.Id);
                    med.listEspecialidades = this.ListarHorariosXEspecialidadXMedico(med.Id);


                    lista.Add(med);
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
        public void Agregar(Medico medico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarMedico");
                datos.setearParametro("@IdPersona", medico.IdPersona);

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
        public void AgregarHorario (int idMedico, Horario hs)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarHorarioXMedico");
                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@idHorario", hs.Id);

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
        public void AgregarEspecialidad (int idMedico, Especialidad especialidad)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarEspecialidadXMedico");
                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@idEspecialidad", especialidad.Id);

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
        public List<Horario> ListarHorariosXMedico (int id)
        {
            List<Horario> lista = new List<Horario>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT H.ID as IDHORARIO, H.DIA AS DIAHORARIO, H.HORARIOINICIO AS HORARIOINICIO, H.HORARIOFIN AS HORARIOFIN FROM MEDICOXHORARIO MXH inner join HORARIOS H On h.ID = MXH.IDHORARIO WHERE MXH.IDMEDICO = " + id.ToString();
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Horario horario = new Horario();

                    horario.Id = (int)lector["IDHORARIO"];
                    horario.Dia = (string)lector["DIAHORARIO"];
                    horario.HoraInicio = (string)lector["HORARIOINICIO"];
                    horario.HoraFin = (string)lector["HORARIOFIN"];


                    lista.Add(horario);
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
        public List<Especialidad> ListarHorariosXEspecialidadXMedico (int id)
        {
            List<Especialidad> lista = new List<Especialidad>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT E.ID as ID, E.ESPECIALIDAD AS ESPE FROM MEDICOXESPECIALIDAD MXE INNER JOIN ESPECIALIDADES E ON E.ID = MXE.IDESPECIALIDAD WHERE MXE.IDMEDICO = " + id.ToString();
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Especialidad horario = new Especialidad();

                    horario.Id = (int)lector["ID"];
                    horario.especialidad = (string)lector["ESPE"];

                    lista.Add(horario);
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

        public void EliminarHorariosXmedico(int idMedico, Horario horario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_EliminarHorario");
                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@idHorario", horario.Id);

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

        public void EliminarEspecilidadXmedico (int idMedico, Especialidad espe)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_EliminarEspecialidad");
                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@IdEspecialidad", espe.Id);

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

        public void Eliminar(Medico medico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                foreach (var item in medico.listEspecialidades)
                {
                    EliminarEspecilidadXmedico(medico.Id, item);
                }
                foreach (var item in medico.listHorarios)
                {
                    EliminarHorariosXmedico(medico.Id, item);
                }

                EliminaMedico(medico.Id, datos);
  

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

        private void EliminaMedico(int id, AccesoDatos datos)
        {
            datos.setearProcedimiento("SP_EliminarMedico");
            datos.setearParametro("@ID", id);

            datos.ejectutarAccion();
        }
    }
}
