
using System.Data;

using MySql.Data.MySqlClient;

namespace juegoCartas_net.Models
{
    public class RepositorioCarta : RepositorioBase, IRepositorioCarta
    {
        public RepositorioCarta(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Carta p)
        {
           	int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"INSERT INTO `carta`( `personaje_id`, `mazo_id`) VALUES (@personaje_id, @mazo_id);
					SELECT LAST_INSERT_ID();";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@personaje_id", p.PersonajeId);
					command.Parameters.AddWithValue("@mazo_id", p.MazoId);
					// command.Parameters.AddWithValue("@nivel", p.Nivel);
                    // command.Parameters.AddWithValue("@puntos_habilidad", p.PuntosHabilidad);
					// command.Parameters.AddWithValue("@imagen", p.Imagen);
				

					res = Convert.ToInt32(command.ExecuteScalar());
					p.Id = res;
				
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

        public Carta ObtenerPorId(int id)
        {
             Carta? e = null;
			    MySqlConnection conn = ObtenerConexion();
			{
				string sql = @" SELECT carta.id, `personaje_id`, `mazo_id`, carta.nivel, p.puntos_habilidad,  p.imagen, p.nombre, cuerpo.vida, cara.tipo, cabeza.ataque
                    FROM `carta`
                    join mazo m on `mazo_id` = m.id
                    join usuario u on m.usuario_id = u.id
                    join personaje p on `personaje_id` = p.id
                    Join Cabeza cabeza on cabeza.id = p.cabezaId 
                    Join Cuerpo cuerpo on cuerpo.id = p.cuerpoId
                    Join Cara cara on cara.id = p.caraId
                    WHERE carta.id =@id";
				   using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@id", id);
					command.CommandType = CommandType.Text;
					
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Carta
						{
							  Id = reader.GetInt32(0),
                            PersonajeId = reader.GetInt32("personaje_id"),
                            MazoId = reader.GetInt32("mazo_id"),
                            Imagen = reader.GetString("imagen"),
                            PersonajeNombre = reader.GetString("nombre"),
                            PuntosHabilidad = reader.GetInt32(4),
                            Vida = reader.GetInt32("vida"),
                            Ataque = reader.GetInt32("ataque"),
                            Tipo = reader.GetInt32("tipo"),		
						};
					}
				
				}
			}
			return e;
        }
 public IList<Carta> ObtenerPorIdUsuarioRandom(int id)
        {
            IList<Carta> res = new List<Carta>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT 
    c.id AS carta_id, 
    c.personaje_id, 
    c.mazo_id, 
    c.nivel, 
    p.puntos_habilidad,  
    p.imagen, 
    p.nombre, 
    cuerpo.vida, 
    cara.tipo, 
    cabeza.ataque
FROM (
    SELECT carta.id 
    FROM carta
    JOIN mazo m ON carta.mazo_id = m.id
    JOIN usuario u ON m.usuario_id = u.id
    WHERE u.id =@id
    ORDER BY RAND()
    LIMIT 3
) AS t2
JOIN carta c ON c.id = t2.id
JOIN mazo m ON c.mazo_id = m.id
JOIN usuario u ON m.usuario_id = u.id
JOIN personaje p ON c.personaje_id = p.id
JOIN cabeza cabeza ON cabeza.id = p.cabezaId
JOIN cuerpo cuerpo ON cuerpo.id = p.cuerpoId
JOIN cara cara ON cara.id = p.caraId;
 ";
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
                            PuntosHabilidad = reader.GetInt32(4),
                            Vida = reader.GetInt32("vida"),
                            Ataque = reader.GetInt32("ataque"),
                            Tipo = reader.GetInt32("tipo")

                        };

                        res.Add(e);
                    }
                }
            }
            return res;
        }

        public IList<Carta> ObtenerPorIdUsuario(int id)
        {
            IList<Carta> res = new List<Carta>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT carta.id, `personaje_id`, `mazo_id`, carta.nivel, p.puntos_habilidad,  p.imagen, p.nombre, cuerpo.vida, cara.tipo, cabeza.ataque
                    FROM `carta`
                    join mazo m on `mazo_id` = m.id
                    join usuario u on m.usuario_id = u.id
                    join personaje p on `personaje_id` = p.id
                    Join Cabeza cabeza on cabeza.id = p.cabezaId 
                    Join Cuerpo cuerpo on cuerpo.id = p.cuerpoId
                    Join Cara cara on cara.id = p.caraId
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
                            PuntosHabilidad = reader.GetInt32(4),
                            Vida = reader.GetInt32("vida"),
                            Ataque = reader.GetInt32("ataque"),
                            Tipo = reader.GetInt32("tipo")

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
    }
}