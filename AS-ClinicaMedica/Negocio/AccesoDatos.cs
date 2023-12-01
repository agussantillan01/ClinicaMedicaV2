using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class AccesoDatos
    {
            private SqlConnection conexion;
            private SqlCommand comando;
            private SqlDataReader lector;
            public SqlDataReader Lector
            {
                get { return lector; }
            }
            public AccesoDatos()
            {
                conexion = new SqlConnection("server=.\\SQLEXPRESS; database=AS_DB_CLINICA; integrated security= true");
                comando = new SqlCommand();
            }

            public void setearConsulta(string consulta)
            {
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = consulta;
            }
            public void setearProcedimiento(string sp)
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = sp;
            }

            public void setearParametro(string nombre, object valor)
            {
                comando.Parameters.AddWithValue(nombre, valor);
            }


            public void ejecutarLectura()
            {
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();
            }

            public void cerrarConexion()
            {
                if (lector != null)
                    lector.Close();
                conexion.Close();
            }



            internal void ejectutarAccion()
            {
                comando.Connection = conexion;
                conexion.Open();
                comando.ExecuteNonQuery();
            }

            public int ejecutarAccionScalar()
            {
                comando.Connection = conexion;
                conexion.Open();
                return int.Parse(comando.ExecuteScalar().ToString());
            }
    }
    
}
