
using MySql.Data.MySqlClient;
using System.Data;

namespace juegoCartas_net.Models
{
    public class RepositorioCabeza : RepositorioBase, IRepositorioCabeza
    {
        public RepositorioCabeza(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Cabeza p)
        {
          	int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"INSERT INTO `cabeza`( `imagen`, `caracteristica`, `nombre`, ataque) VALUES (@imagen,@caracteristica,@nombre, @ataque);
					SELECT LAST_INSERT_ID();";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					
					command.Parameters.AddWithValue("@imagen", p.Imagen);
					command.Parameters.AddWithValue("@caracteristica", p.Caracteristica);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@ataque", p.Ataque);

					res = Convert.ToInt32(command.ExecuteScalar());
					p.Id = res;
				
				}
			}
			conn.Close();
			return res;
        }

       public int Baja(int id)
        {int res = -1;
			  MySqlConnection conn = ObtenerConexion();
			{
				string sql = "DELETE FROM `cabeza` WHERE id = @id";
				 using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					res = command.ExecuteNonQuery();
				
				}
				
			}
			conn.Close();
			return res;
          
        }
        public int Modificacion(Cabeza entidad)
		{
			int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = "UPDATE `Cabeza` SET `imagen`=@imagen,`caracteristica`=@caracteristica,`nombre`=@nombre, ataque=@ataque WHERE id= @id";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@imagen", entidad.Imagen);
					command.Parameters.AddWithValue("@caracteristica", entidad.Caracteristica);
					command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                    command.Parameters.AddWithValue("@ataque", entidad.Ataque);
                    command.Parameters.AddWithValue("@id", entidad.Id);
					command.CommandType = CommandType.Text;

					res = command.ExecuteNonQuery();

				}
			}
			conn.Close();
			return res;
        }

        public Cabeza ObtenerPorId(int id)
        {

            Cabeza? e = null;
			    MySqlConnection conn = ObtenerConexion();
			{
				string sql = @" SELECT `id`, `imagen`, `caracteristica`, `nombre`, ataque FROM `Cabeza` WHERE id=@id";
				   using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@id", id);
					command.CommandType = CommandType.Text;
					
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Cabeza
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
                            Caracteristica = reader.GetString("caracteristica"),
                            Ataque = reader.GetInt32("ataque"),
							Imagen = reader["imagen"] == DBNull.Value ? "" : reader.GetString("imagen"),		
						};
					}
				
				}
			}
			conn.Close();
			return e;

           
        }

        public IList<Cabeza> ObtenerTodos()
        {
            IList<Cabeza> res = new List<Cabeza>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT `id`, `imagen`, `caracteristica`, `nombre`, ataque FROM `Cabeza`";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Cabeza e = new Cabeza
                        {
                            Id = reader.GetInt32("Id"),
                            Imagen = reader.IsDBNull(3) ? null : reader.GetString(1),
                            Caracteristica = reader.GetString("caracteristica"),
                            Nombre = reader.GetString("nombre"),
                            Ataque = reader.GetInt32("ataque"),
                        
                        };
                        res.Add(e);
                    }

                }
            }
			conn.Close();
            return res;
        }
    }
    }
