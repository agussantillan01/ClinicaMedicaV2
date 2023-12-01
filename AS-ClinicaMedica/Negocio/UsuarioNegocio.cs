using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class UsuarioNegocio
    {
        public List<Usuario> listar()
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT U.ID AS IDUSER, U.USUARIO AS USERNAME, U.CONTRASENIA AS CLAVE, U.IDTIPOUSER AS IDTIPO, T.TIPO AS TIPO FROM USUARIOS U INNER JOIN TIPOUSER T ON T.ID = U.IDTIPOUSER";
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Usuario es = new Usuario();
                    es.Id = (int)lector["IDUSER"];
                    es.Nombre = (string)lector["USERNAME"];
                    es.Contrasenia = (string)lector["CLAVE"];
                    es.perfil = new TipoUsuario();
                    es.perfil.Id = (int)lector["IDTIPO"];
                    es.perfil.Tipo = (string)lector["TIPO"];

                    lista.Add(es);
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

        public void Guardar(Usuario us)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarUsuario");
                datos.setearParametro("@Usuario", us.Nombre);
                datos.setearParametro("@Contrasenia", us.Contrasenia);
                datos.setearParametro("@TipoUser", us.perfil.Id);

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

        public void ModificarContrasenia(int idPersona, string clave)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_ActualizarClaveUsuario");
                datos.setearParametro("@IdUsuario", idPersona);
                datos.setearParametro("@Contrasenia", clave);

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

        public void Actualizar(Usuario us, int idUser)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_ActualizarUsuario");
                datos.setearParametro("@IdUsuario", idUser);
                datos.setearParametro("@Usuario", us.Nombre);
                datos.setearParametro("@Clave", us.Contrasenia);

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

        public void Eliminar(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_EliminarUsuario");
                datos.setearParametro("@Id", usuario.Id);

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
