using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //creo sql client para la conexion

namespace negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion; //declaro variables vacias de los atributos
        private SqlCommand comando;
        private SqlDataReader lector; //propiedad/atributo privada 
        public  SqlDataReader Lector //aca va en mayuscula asi lo puedo leer desde el exterior
            {
            get { return lector; } //aca minuscula 
            }

        public AccesoDatos() //creo constructor que cuando nace con la conexion y le paso x parametro el server a donde se va a conectar y tambien nace con un comando
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true");
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta) //funcion para comando por parametro
        {
            comando.CommandType = System.Data.CommandType.Text;// encapsulo darle tipo
            comando.CommandText = consulta;//dar la consulta
        }
        public void ejecutarLectura()// dar conexion, abre y ejecuta y esa ejecucion va a devolver un lector
        {
            comando.Connection = conexion; //conecta
            try // dentro de un try y catch para q no se rompa
            {
                conexion.Open();// abre la conexion
                lector = comando.ExecuteReader(); //ejecuta esa conexion devolviendo una instancia de un objeto de tipo sql data reader 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ejecutarAccion() //creo metodo para insertar 
        {
            comando.Connection = conexion;//conecto
            try
            {
                conexion.Open();// abro la conexion
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void setearParametro  (string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);//el comando recibe x parametro x valor 
        }


        public void cerrarConexion () // funcion para cerrar conexiones
        {
            if (lector != null)
                lector.Close(); // se cierra el lector si hay un lector utilizandose 
            conexion.Close();
        }
    }
}
