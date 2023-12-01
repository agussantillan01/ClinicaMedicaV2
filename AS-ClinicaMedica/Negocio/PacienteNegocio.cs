using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PacienteNegocio
    {
        public void Agregar(Dominio.Paciente pac)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarPaciente");
                datos.setearParametro("@FechaNacimiento", pac.FechaNacimiento);
                datos.setearParametro("@Direccion", pac.Direccion);
                datos.setearParametro("@Contacto", pac.Contacto);
                datos.setearParametro("@IdPersona", pac.IdPersona);

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

        public List<Paciente> ListarPacientes()
        {
            List<Paciente> lista = new List<Paciente>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT P.ID as 'PacienteId', DP.NOMBRE, DP.APELLIDO, DP.DNI, DP.EMAIL, DP.SEXO, P.DIRECCION, P.CONTACTO, P.FECHANACIMIENTO, dp.ID as 'personaID' FROM USUARIOSDATOSPERSONALES DP INNER JOIN PACIENTES P ON P.ID_USUARIODATOPERSONAL= DP.ID";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();
                while (lector.Read())
                { 
                    Paciente paciente = new Paciente();
                    paciente.IdPaciente = (int)lector["PacienteId"];
                    paciente.FechaNacimiento = (DateTime)lector["FECHANACIMIENTO"];
                    paciente.Direccion = (string)lector["DIRECCION"];
                    paciente.Contacto = (string)lector["CONTACTO"];
                    paciente.Nombre = (string)lector["NOMBRE"];
                    paciente.Apellido = (string)lector["APELLIDO"];
                    paciente.Sexo = (string)lector["SEXO"];
                    paciente.DNI = (string)lector["DNI"];
                    paciente.Email = (string)lector["EMAIL"];
                    paciente.IdPersona = (int)lector["personaID"];

                    lista.Add(paciente);
                }


                return lista;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                conexion.Close();
            }
        }
        public void Actualizar(Paciente pac)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_ActualizarPaciente");
                datos.setearParametro("@IdPaciente", pac.IdPaciente);
                datos.setearParametro("@FechaNacimiento", pac.FechaNacimiento);
                datos.setearParametro("@Direccion", pac.Direccion);
                datos.setearParametro("@Contacto", pac.Contacto);
                datos.setearParametro("@IdPersona", pac.IdPersona);
                datos.setearParametro("@Nombre", pac.Nombre);
                datos.setearParametro("@Apellido", pac.Apellido);
                datos.setearParametro("@Dni", pac.DNI);
                datos.setearParametro("@Sexo", pac.Sexo);
                datos.setearParametro("@Email", pac.Email);



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

        public void Eliminar(Paciente pac)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_EliminarPaciente");
                datos.setearParametro("@Id", pac.IdPaciente);

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
