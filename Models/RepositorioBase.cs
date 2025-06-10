using MySql.Data.MySqlClient;
using System;


namespace juegoCartas_net.Models
{
    public abstract class RepositorioBase
    {
        private readonly string myConnectionString = "Server=localhost;Database=juegocartas;Uid=root;Pwd=";

       
        public RepositorioBase(IConfiguration configuration)
        {
            
        }

              protected MySqlConnection ObtenerConexion()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(myConnectionString);
                conn.Open();
                return conn;
            }
            catch (MySqlException ex)
            {
               
                return null; 
            }
        }

       
        protected void CerrarConexion(MySqlConnection conn)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
