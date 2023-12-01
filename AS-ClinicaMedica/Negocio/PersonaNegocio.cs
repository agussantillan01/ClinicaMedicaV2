using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PersonaNegocio
    {
        public List<Dominio.Persona> Listar()
        {
            List<Persona> lista = new List<Persona>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT ID, NOMBRE, APELLIDO, DNI, EMAIL, SEXO, IDUSUARIO FROM USUARIOSDATOSPERSONALES";
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Persona persona = new Persona();

                    persona.IdPersona = (int)lector["ID"];
                    persona.Nombre = (string)lector["NOMBRE"];
                    persona.Apellido = (string)lector["APELLIDO"];
                    persona.DNI = (string)lector["DNI"];
                    persona.Email = (string)lector["EMAIL"];
                    persona.Sexo = (string)lector["SEXO"];
                    if (lector["IDUSUARIO"] is DBNull)
                    {
                        persona.usuario = null;
                    }
                    else
                    {
                        persona.usuario = ListarUsuarios().FirstOrDefault(x => x.Id == (int)lector["IDUSUARIO"]);
                    }
                    


                    lista.Add(persona);
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

        private List<Dominio.Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT ID, USUARIO, CONTRASENIA, IDTIPOUSER FROM USUARIOS";
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Usuario usuario = new Usuario();

                    usuario.Id = (int)lector["ID"];
                    usuario.Nombre = (string)lector["USUARIO"];
                    usuario.Contrasenia = (string)lector["CONTRASENIA"];
                    usuario.perfil = ListarPerfiles().FirstOrDefault(x => x.Id == (int)lector["IDTIPOUSER"]);
                    
                    lista.Add(usuario);
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
        private List<Dominio.TipoUsuario> ListarPerfiles()
        {
            List<TipoUsuario> lista = new List<TipoUsuario>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT ID, TIPO FROM TIPOUSER";
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    TipoUsuario tipoUsuario = new TipoUsuario();

                    tipoUsuario.Id = (int)lector["ID"];
                    tipoUsuario.Tipo = (string)lector["TIPO"];
                    lista.Add(tipoUsuario);
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
        public void Agregar(Dominio.Persona per)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarPersona");
                datos.setearParametro("@Nombre", per.Nombre);
                datos.setearParametro("@Apellido", per.Apellido);
                datos.setearParametro("@Dni", per.DNI);
                datos.setearParametro("@Email", per.Email);
                datos.setearParametro("@Sexo", per.Sexo);
                if (per.usuario != null)
                {
                    datos.setearParametro("@IdUsuario", per.usuario.Id);
                } else datos.setearParametro("@IdUsuario", DBNull.Value);



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
        public void Actualizar(Dominio.Persona per)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_ActualizarPersona");
                datos.setearParametro("@IdPersona", per.IdPersona);
                datos.setearParametro("@Nombre", per.Nombre);
                datos.setearParametro("@Apellido", per.Apellido);
                datos.setearParametro("@Dni", per.DNI);
                datos.setearParametro("@Email", per.Email);
                datos.setearParametro("@Sexo", per.Sexo);



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


        public void Eliminar(Persona per)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_EliminarPersona");
                datos.setearParametro("@Id", per.IdPersona);

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
