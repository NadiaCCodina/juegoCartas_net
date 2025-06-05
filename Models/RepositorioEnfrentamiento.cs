
using System.Data;

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
          
        public IList<Carta> ObtenerPorIdUsuario(int id)
        {
            IList<Carta> res = new List<Carta>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT carta.id, `personaje_id`, `mazo_id`, carta.nivel, carta.puntos_habilidad,  p.imagen, p.nombre
                    FROM `carta`
                    join mazo m on `mazo_id` = m.id
                    join usuario u on m.usuario_id = u.id
                    join personaje p on `personaje_id` = p.id
                    WHERE u.id =@id";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Carta e = new Carta
                        {
                            Id = reader.GetInt32(0),
                            PersonajeId = reader.GetInt32("personaje_id"),
                            MazoId = reader.GetInt32("mazo_id"),
                            Imagen = reader.GetString("imagen"),
                            PersonajeNombre = reader.GetString("nombre"),
                        };
                        res.Add(e);
                    }
                }
            }
            return res;
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