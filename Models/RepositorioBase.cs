using MySql.Data.MySqlClient;
using System;


namespace juegoCartas_net.Models
{
    public abstract class RepositorioBase
    {
        private readonly string myConnectionString = "Server=localhost;Database=juegocartas;Uid=root;Pwd=";

        // Constructor de la clase (opcional, si lo necesitas)
        public RepositorioBase(IConfiguration configuration)
        {
            // Puedes inicializar algo aquí si es necesario
        }

        // Método para obtener la conexión a la base de datos
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
                // Puedes mostrar el error o manejarlo de la forma que desees

                return null; // Retorna null en caso de error
            }
        }

        // Método para cerrar la conexión (se debe usar en conjunto con 'using' para garantizar el cierre correcto)
        protected void CerrarConexion(MySqlConnection conn)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
