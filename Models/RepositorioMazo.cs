
using System.Data;

using MySql.Data.MySqlClient;

namespace juegoCartas_net.Models
{
    public class RepositorioMazo : RepositorioBase, IRepositorioMazo
    {
        public RepositorioMazo(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Usuario u)
        {
           	int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"INSERT INTO `mazo`(`usuario_id`, `puntos_habilidad`) VALUES (@usuario_id, @puntos_habilidad);
					SELECT LAST_INSERT_ID();";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@usuario_id", u.Id);
					command.Parameters.AddWithValue("@puntos_habilidad", 25);
					res = Convert.ToInt32(command.ExecuteScalar());
					u.Id = res;
				
				}
			}
			return res;
        }

        public int Alta(Mazo p)
        {
            throw new NotImplementedException();
        }

        public int Baja(int id)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Carta p)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Mazo p)
        {
            throw new NotImplementedException();
        }

        public Mazo ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Mazo ObtenerPorEmailUsuario(string email)
        {
          Mazo? m = null;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT mazo.id, `usuario_id`, `puntos_habilidad` FROM `mazo`
                    JOIN usuario u ON u.id =`usuario_id`
                    WHERE u.email = @email";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                      m = new Mazo
                        {
                            Id = reader.GetInt32(0),
                            UsuarioId = reader.GetInt32("usuario_id"),
                             };
                    
                    }
                }
            }
             return m;
        }

        public IList<Carta> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        Mazo IRepositorio<Mazo>.ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        IList<Mazo> IRepositorio<Mazo>.ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}