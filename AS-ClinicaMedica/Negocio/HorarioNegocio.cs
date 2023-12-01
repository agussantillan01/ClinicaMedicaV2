using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class HorarioNegocio
    {
        public List<Horario> Listar()
        {
            List<Horario> lista = new List<Horario>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT ID, DIA, HORARIOINICIO, HORARIOFIN FROM HORARIOS";
                comando.Connection = conexion;
                conexion.Open();

                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Horario es = new Horario();
                    es.Id = (int)lector["ID"];
                    es.Dia = (string)lector["DIA"];
                    es.HoraInicio = (string)lector["HORARIOINICIO"];
                    es.HoraFin = (string)lector["HORARIOFIN"];


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
        public void Agregar(Horario hs)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_AgregarHorario");
                datos.setearParametro("@Dia", hs.Dia);
                datos.setearParametro("@HoraInicio", hs.HoraInicio);
                datos.setearParametro("@HoraFin", hs.HoraFin);

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
        public void Actualizar(Horario hs)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_ActualizarHorario");
                datos.setearParametro("@Id", hs.Id);
                datos.setearParametro("@Dia", hs.Dia);
                datos.setearParametro("@HoraInicio", hs.HoraInicio);
                datos.setearParametro("@HoraFin", hs.HoraFin);

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

        public void Eliminar(Horario hs)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("SP_EliminaHorario");
                datos.setearParametro("@Id", hs.Id);

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
