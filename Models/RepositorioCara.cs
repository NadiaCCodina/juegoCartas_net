

using System.Data;

using MySql.Data.MySqlClient;

namespace juegoCartas_net.Models
{
    public class RepositorioCara : RepositorioBase, IRepositorioCara
    {
        public RepositorioCara(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Cara p)
        {
       	int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"INSERT INTO `cara`( `imagen`, `caracteristica`, `nombre`, tipo) VALUES (@imagen,@caracteristica,@nombre, @tipo);
					SELECT LAST_INSERT_ID();";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@imagen", p.Imagen);
					command.Parameters.AddWithValue("@caracteristica", p.Caracteristica);
                  
					command.Parameters.AddWithValue("@tipo", p.Tipo);
				
				

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

         public int Modificacion(Cara entidad)
		{
			int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = "UPDATE `Cara` SET `imagen`=@imagen,`caracteristica`=@caracteristica,`nombre`=@nombre, tipo= @tipo WHERE id= @id";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@imagen", entidad.Imagen);
					command.Parameters.AddWithValue("@caracteristica", entidad.Caracteristica);
					command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                    command.Parameters.AddWithValue("@id", entidad.Id);
                    
					command.Parameters.AddWithValue("@tipo", entidad.Tipo);
					command.CommandType = CommandType.Text;

					res = command.ExecuteNonQuery();

				}
			}
			return res;
        }

        public Cara ObtenerPorId(int id)
        {

            Cara? e = null;
			    MySqlConnection conn = ObtenerConexion();
			{
				string sql = @" SELECT `id`, `imagen`, `caracteristica`, `nombre`, tipo FROM `Cara` WHERE id=@id";
				   using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@id", id);
					command.CommandType = CommandType.Text;
					
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Cara
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
                            Caracteristica = reader.GetString("caracteristica"),
							Tipo = reader.GetInt32("tipo"),
							Imagen = reader["imagen"] == DBNull.Value ? "" : reader.GetString("imagen"),		
						};
					}
				
				}
			}
			return e;

           
        }

       public IList<Cara> ObtenerTodos()
        {
            IList<Cara> res = new List<Cara>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT `id`, `imagen`, `caracteristica`, `nombre`, tipo FROM `cara`";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Cara e = new Cara
                        {
                            Id = reader.GetInt32("Id"),
                            Imagen = reader.IsDBNull(3) ? null : reader.GetString(1),
                            Caracteristica = reader.GetString("caracteristica"),
                            Nombre = reader.GetString("nombre"),
                            Tipo = reader.GetInt32("tipo"),
                      
                        
                        };
                        res.Add(e);
                    }

                }
            }
            return res;
        }
    }
    }
