using System.Data;
using MySql.Data.MySqlClient;

namespace juegoCartas_net.Models
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {

        }

        public IList<Usuario> ObtenerTodos()
        {
            IList<Usuario> res = new List<Usuario>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol
					FROM Usuario
                    ";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuario e = new Usuario
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                          
                            Avatar = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Email = reader.GetString("Email"),
                            Clave = reader.GetString("Clave"),
                            Rol = reader.GetInt32("Rol"),
                        };
                        res.Add(e);
                    }

                }
            }
            return res;
        }

   public Usuario ObtenerRandom()
        {
			Usuario random = null;
			    MySqlConnection conn = ObtenerConexion();
			{
				string sql = @" SELECT t.id FROM Usuario t JOIN 
				(SELECT(FLOOR(max(id) * rand())) as maxid FROM Usuario)
				 as tt on t.id >= tt.maxid LIMIT 1";
				   using (var command = new MySqlCommand(sql, conn))
				{
			
					command.CommandType = CommandType.Text;
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						random = new Usuario
						{
							Id = reader.GetInt32("Id"),
							
						};
					}
				}
			}
			return random;
        }

		public int Alta(Usuario e)
		{
			int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"INSERT INTO Usuario 
					(Nombre,  Avatar, Email, Clave, Estado) 
					VALUES (@nombre,  @avatar, @email, @clave,  1);
					Select LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);

					if (String.IsNullOrEmpty(e.Avatar))
						command.Parameters.AddWithValue("@avatar", DBNull.Value);
					else
						command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);


					res = Convert.ToInt32(command.ExecuteScalar());
					e.Id = res;

				}
			}
			return res;
		}

        public int Modificacion(Usuario e)
        {
            int res = -1;
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"UPDATE Usuario 
					SET Nombre=@nombre, Avatar=@avatar, Clave=@clave 
					WHERE Id = @id";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", e.Nombre);
                
                    command.Parameters.AddWithValue("@avatar", e.Avatar);
                    
                    command.Parameters.AddWithValue("@clave", e.Clave);
                   
                    command.Parameters.AddWithValue("@id", e.Id);

                    res = command.ExecuteNonQuery();

                }
            }
            return res;
        }
        	public int Baja(int id)
		{
			int res = -1;
			  MySqlConnection conn = ObtenerConexion();
			{
				string sql = "DELETE FROM `usuario` WHERE `id`= @id";
				 using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					res = command.ExecuteNonQuery();
				
				}
			}
			return res;
		}
        
		public Usuario ObtenerPorId(int id)
		{
			Usuario? e = null;
			    MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"SELECT 
					Id, Nombre,  Avatar,  Clave
					FROM Usuario
					WHERE Id=@id";
				   using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@id", id);
					command.CommandType = CommandType.Text;
					
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
						
							Avatar = reader["Avatar"] == DBNull.Value ? "" : reader.GetString("Avatar"),
						
							Clave = reader.GetString("Clave"),
						
						};
					}
				
				}
			}
			return e;
		}

		public Usuario ObtenerPorEmail(string email)
		{
			Usuario? e = null;
				    MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"SELECT
					Id, Nombre,  Avatar, Email, Clave, Rol FROM Usuario
					WHERE Email=@email";
			 using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
                    	command.Parameters.AddWithValue("@email",  email);
                 
			
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
						
                            Avatar = reader["Avatar"] == DBNull.Value ? "" : reader.GetString("Avatar"),
                            Email = reader.GetString("Email"),
							Clave = reader.GetString("Clave"),
							Rol = reader.GetInt32("Rol"),
						};
					}
				
				}
			}
			return e;
		}

        
      
    }
}

