using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class PokemonNegocio
    {
        public List<Pokemon> listar()
        {
            List<Pokemon> lista = new List<Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Numero, Nombre, P.Descripcion, UrlImagen,E.Descripcion Tipo, D.Descripcion debilidad, P.IdTipo, P.IdDebilidad, P.Id  from POKEMONS P, ELEMENTOS E, ELEMENTOS D where e.Id= p.IdTipo AND D.Id= P.IdDebilidad And P.Activo = 1";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    if (!lector.IsDBNull(lector.GetOrdinal("UrlImagen")))//para q no falle si hay un nulo
                    aux.UrlImagen = (string)lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.id = (int)lector["idTipo"];//Agrego los elemntos que traje de base de datos q modidifique en la consulta
                    aux.Tipo.Descripcion = (string)lector["Tipo"];// descripcion del id
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.id = (int)lector["idDebilidad"];//id
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];


                    lista.Add(aux);
                }

                conexion.Close();
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

        public void agregar(Pokemon nuevo)//logica de conoxion a la base de datos, El método agregar (Pokemon nuevo) está pensado para recibir un objeto Pokemon llamado nuevo, y con los datos que contiene ese objeto (como número, nombre, descripción, tipo, debilidad, etc.), lo inserta en la base de datos.
        {
            AccesoDatos datos = new AccesoDatos();// Creo objecto de acceso a datos 
            try
            {

                datos.setearConsulta("insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad,UrlImagen ) values (" + nuevo.Numero + ", '" + nuevo.Nombre + "' ,'" + nuevo.Descripcion + "', 1, @idTipo,@idDebilidad,@urlimagen)");//creo una variable idtipo y id debilidad con el @(se llamn parametros).
                datos.setearParametro("@idTipo", nuevo.Tipo.id);// utilizo el metodo setear parametro q me pide un nombre y el valor que va a tener esa fila, nuevo se llama el metodo ejemplo del pokemon quiero un nuevo objecto q es  nombre y asi.
                datos.setearParametro("@iddebilidad", nuevo.Debilidad.id);// cuando se ejecute va a reemplazar en el @ por los valores q saca de la tabla 
                datos.setearParametro("@urlimagen", nuevo.UrlImagen);//lo agrego aca
                datos.ejecutarAccion();//cargar esa informacion
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

        public void  modificar (Pokemon poke)
         {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @descripcion, UrlImagen = @urlimagen, IdTipo = @idTipo, IdDebilidad = @idDebilidad Where Id = @id");
                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@descripcion", poke.Descripcion);
                datos.setearParametro("@urlimagen", poke.UrlImagen);
                datos.setearParametro("@idTipo", poke.Tipo.id);
                datos.setearParametro("@idDebilidad", poke.Debilidad.id);
                datos.setearParametro("@id", poke.Id);

                datos.ejecutarAccion();

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

        public List<Pokemon> filtrar (string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select Numero, Nombre, P.Descripcion, UrlImagen,E.Descripcion Tipo, D.Descripcion debilidad, P.IdTipo, P.IdDebilidad, P.Id  from POKEMONS P, ELEMENTOS E, ELEMENTOS D where e.Id= p.IdTipo AND D.Id= P.IdDebilidad And P.Activo = 1 And ";
                if (campo == "Número")
                {
                    switch (criterio)

                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                        break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "P.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("UrlImagen")))//para q no falle si hay un nulo
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.id = (int)datos.Lector["idTipo"];//Agrego los elemntos que traje de base de datos q modidifique en la consulta
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];// descripcion del id
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.id = (int)datos.Lector["idDebilidad"];//id
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];


                    lista.Add(aux);
                }


                return lista;
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


        public void eliminar(int id) 
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.setearConsulta("delete from POKEMONS where id= @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
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

        public void eliminarLogico(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Activo = 0 where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
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

        
}   }
