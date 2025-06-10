
using System.Data;
using System.Text.Json;
using MySql.Data.MySqlClient;

namespace juegoCartas_net.Models
{
    public class RepositorioEnfrentamiento : RepositorioBase, IRepositorioEnfrentamiento
    {
        public RepositorioEnfrentamiento(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Enfrentamiento e)
        {
            int res = -1;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"INSERT INTO `enfrentamiento`( `retador_id`, `contrincante_id`, `fecha`, resultado) 
                VALUES (@retador_id, @contrincante_id, NOW(),  @resultado);
				SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@retador_id", e.RetadorId);
                    command.Parameters.AddWithValue("@contrincante_id", e.ContrincanteId);
                    command.Parameters.AddWithValue("@resultado", e.Resultado);

                    res = Convert.ToInt32(command.ExecuteScalar());
                    e.Id = res;

                }
            }
            conn.Close();
            return res;
        }



        public int Baja(int id)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Carta p)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Enfrentamiento p)
        {
            throw new NotImplementedException();
        }

        public Carta ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
        public string ObtenerResultadosJson(int retadorId)
        {
            var resultados = new List<Dictionary<string, object>>();
            MySqlConnection conn = ObtenerConexion();
            {

                string query = @"
                SELECT `retador_id`, u.nombre, resultado, COUNT(resultado) AS total
                FROM `enfrentamiento` 
                JOIN usuario u ON u.id = retador_id
                WHERE retador_id = @retadorId
                GROUP BY resultado 
                ORDER BY total DESC";

                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@retadorId", retadorId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var fila = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                fila[reader.GetName(i)] = reader.GetValue(i);
                            }

                            resultados.Add(fila);
                        }
                    }
                }
            }

            // Serializar a JSON
            return JsonSerializer.Serialize(resultados, new JsonSerializerOptions { WriteIndented = true });
        }


        public IList<Carta> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        Enfrentamiento IRepositorio<Enfrentamiento>.ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        IList<Enfrentamiento> IRepositorio<Enfrentamiento>.ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}