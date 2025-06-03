using System.Data;

using MySql.Data.MySqlClient;

namespace juegoCartas_net.Models
{
    public class RepositorioPersonaje : RepositorioBase, IRepositorioPersonaje
    {
        public RepositorioPersonaje(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Personaje p)
        {
            int res = -1;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"INSERT INTO `personaje`( `caraId`, `cuerpoId`, `cabezaId`, imagen, nombre, puntos_habilidad) VALUES (@caraId, @cuerpoId, @cabezaId, @imagen, @nombre, @puntos_habilidad);
					SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@caraId", p.CaraId);
                    command.Parameters.AddWithValue("@cuerpoId", p.CuerpoId);
                    command.Parameters.AddWithValue("@cabezaId", p.CabezaId);
                    command.Parameters.AddWithValue("@imagen", p.Imagen);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@puntos_habilidad", p.PuntosHabilidad);

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

        public int Modificacion(Personaje entidad)
        {
            int res = -1;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = "UPDATE `Personaje` SET  `caraId`=@caraId, `cuerpoId`=@cuerpoId, `cabezaId`=@cabezaId, imagen=@imagen, nombre=@nombre,  puntos_habilidad= @puntos_habilidad WHERE id= @id";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@caraId", entidad.CaraId);
                    command.Parameters.AddWithValue("@cuerpoId", entidad.CuerpoId);
                    command.Parameters.AddWithValue("@cabezaId", entidad.CabezaId);
                    command.Parameters.AddWithValue("@imagen", entidad.Imagen);
                    command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                     command.Parameters.AddWithValue("@puntos_habilidad", entidad.PuntosHabilidad);
                    command.Parameters.AddWithValue("@id", entidad.Id);
                    command.CommandType = CommandType.Text;

                    res = command.ExecuteNonQuery();

                }
            }
            return res;
        }

        public Personaje ObtenerPorId(int id)
        {

            Personaje? p = null;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @" SELECT `id`, `caraId`, `cuerpoId`, `cabezaId`, `imagen`, `nombre` FROM `Personaje` WHERE id=@id";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Personaje
                        {
                            Id = reader.GetInt32("id"),
                            CaraId = reader.GetInt32("caraId"),
                            CuerpoId = reader.GetInt32("cuerpoId"),
                            CabezaId = reader.GetInt32("cabezaId"),
                            Imagen = reader.GetString("imagen"),
                            Nombre = reader.GetString("nombre"),
                        };
                    }

                }
            }
            return p;


        }
        public IList<Personaje> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
        {
            IList<Personaje> res = new List<Personaje>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @$"
					SELECT p.id, `caraId`, `cuerpoId`, `cabezaId`, p.imagen, p.nombre, Cuerpo.vida, Cara.tipo, Cabeza.ataque,  puntos_habilidad
                    FROM `Personaje` p
                    Join Cabeza cabeza on cabeza.id = p.cabezaId 
                    Join Cuerpo cuerpo on cuerpo.id = p.cuerpoId
                    Join Cara cara on cara.id = p.caraId
                    ORDER BY p.id
                    LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
                    ";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Personaje e = new Personaje
                        {
                            Id = reader.GetInt32("id"),
                            CaraId = reader.GetInt32("caraId"),
                            CuerpoId = reader.GetInt32("cuerpoId"),
                            CabezaId = reader.GetInt32("cabezaId"),
                            Imagen = reader.GetString("imagen"),
                            Nombre = reader.GetString("nombre"),
                            Vida = reader.GetInt32("vida"),
                            Tipo = reader.GetInt32("tipo"),
                            Ataque = reader.GetInt32("ataque"),
                            PuntosHabilidad = reader.GetInt32("puntos_habilidad"),
                        };
                        res.Add(e);
                    }

                }
            }
            return res;
        }

        public int ObtenerCantidad()
        {
            int res = 0;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @$"
					SELECT COUNT(id)
					FROM Personaje
				";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = reader.GetInt32(0);
                    }

                }
            }
            return res;
        }

        public IList<Personaje> ObtenerTodos()
        {
            IList<Personaje> res = new List<Personaje>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT p.id, `caraId`, `cuerpoId`, `cabezaId`, p.imagen, p.nombre, Cuerpo.vida, Cara.tipo, Cabeza.ataque, puntos_habilidad
                    FROM `Personaje` p
                    Join Cabeza cabeza on cabeza.id = p.cabezaId 
                    Join Cuerpo cuerpo on cuerpo.id = p.cuerpoId
                    Join Cara cara on cara.id = p.caraId
                    ORDER BY p.id
                    ";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Personaje e = new Personaje
                        {
                            Id = reader.GetInt32("id"),
                            CaraId = reader.GetInt32("caraId"),
                            CuerpoId = reader.GetInt32("cuerpoId"),
                            CabezaId = reader.GetInt32("cabezaId"),
                            Imagen = reader.GetString("imagen"),
                            Nombre = reader.GetString("nombre"),
                            Vida = reader.GetInt32("vida"),
                            Tipo = reader.GetInt32("tipo"),
                            Ataque = reader.GetInt32("ataque"),
                            PuntosHabilidad = reader.GetInt32("puntos_habilidad"),
                        };
                        res.Add(e);
                    }

                }
            }
            return res;
        }
        public Personaje ObtenerPorParte(int caraId, int cabezaId, int cuerpoId)
        {
            Personaje entidad = null;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT p.id, `caraId`, `cuerpoId`, `cabezaId`, p.imagen, p.nombre, cuerpo.vida, cara.tipo, cabeza.ataque
                    FROM `Personaje` p
                    Join Cabeza cabeza on cabeza.id = p.cabezaId 
                    Join Cuerpo cuerpo on cuerpo.id = p.cuerpoId
                    Join Cara cara on cara.id = p.caraId
                    WHERE `caraId`=@caraId
                    AND `cuerpoId`= @cuerpoId
                    AND `cabezaId`= @cabezaId
                    ";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@caraId", caraId);
                    command.Parameters.AddWithValue("@cabezaId", cabezaId);
                    command.Parameters.AddWithValue("@cuerpoId", cuerpoId);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        entidad = new Personaje
                        {
                            Id = reader.GetInt32("id"),
                            CaraId = reader.GetInt32("caraId"),
                            CuerpoId = reader.GetInt32("cuerpoId"),
                            CabezaId = reader.GetInt32("cabezaId"),
                            Imagen = reader.GetString("imagen"),
                            Nombre = reader.GetString("nombre"),
                            Vida = reader.GetInt32("vida"),
                            Tipo = reader.GetInt32("tipo"),
                            Ataque = reader.GetInt32("ataque"),
                        };

                    }

                }
            }
            return entidad;
        }

    }
}
