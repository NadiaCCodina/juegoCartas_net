using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

using MySql.Data.MySqlClient;

namespace juegoCartas_net.Models
{
    public class RepositorioCuerpo : RepositorioBase, IRepositorioCuerpo
    {
        public RepositorioCuerpo(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Cuerpo p)
        {
           	int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = @"INSERT INTO `cuerpo`( `imagen`, `caracteristica`, `nombre` vida) VALUES (@imagen,@caracteristica,@nombre, @vida);
					SELECT LAST_INSERT_ID();";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					
					command.Parameters.AddWithValue("@imagen", p.Imagen);
					command.Parameters.AddWithValue("@caracteristica", p.Caracteristica);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@vida", p.Vida);
				
				

					res = Convert.ToInt32(command.ExecuteScalar());
					p.Id = res;
				
				}
			}
			return res;
        }

       public int Baja(int id)
        {int res = -1;
			  MySqlConnection conn = ObtenerConexion();
			{
				string sql = "DELETE FROM `cuerpo` WHERE id = @id";
				 using (var command = new MySqlCommand(sql, conn))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					res = command.ExecuteNonQuery();
				
				}
			}
			return res;
         
        }

        public int Modificacion(Cuerpo entidad)
		{
			int res = -1;
			MySqlConnection conn = ObtenerConexion();
			{
				string sql = "UPDATE `Cuerpo` SET `imagen`=@imagen,`caracteristica`=@caracteristica,`nombre`=@nombre, vida= @vida WHERE id= @id";
				using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@imagen", entidad.Imagen);
					command.Parameters.AddWithValue("@caracteristica", entidad.Caracteristica);
					command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                    command.Parameters.AddWithValue("@vida", entidad.Vida);
                    command.Parameters.AddWithValue("@id", entidad.Id);
					command.CommandType = CommandType.Text;

					res = command.ExecuteNonQuery();

				}
			}
			return res;
        }

        public Cuerpo ObtenerPorId(int id)
        {

            Cuerpo? e = null;
			    MySqlConnection conn = ObtenerConexion();
			{
				string sql = @" SELECT `id`, `imagen`, `caracteristica`, `nombre`, vida FROM `cuerpo` WHERE id=@id";
				   using (var command = new MySqlCommand(sql, conn))
				{
					command.Parameters.AddWithValue("@id", id);
					command.CommandType = CommandType.Text;
					
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Cuerpo
						{
							Id = reader.GetInt32("Id"),
                            Vida = reader.GetInt32("vida"),
							Nombre = reader.GetString("Nombre"),
                            Caracteristica = reader.GetString("caracteristica"),
							Imagen = reader["imagen"] == DBNull.Value ? "" : reader.GetString("imagen"),		
						};
					}
				
				}
			}
			return e;

           
        }

          public IList<Cuerpo> ObtenerTodos()
        {
            IList<Cuerpo> res = new List<Cuerpo>();
            MySqlConnection conn = ObtenerConexion();
            {
                string sql = @"
					SELECT `id`, `imagen`, `caracteristica`, `nombre`, vida FROM `Cuerpo`";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Cuerpo e = new Cuerpo
                        {
                            Id = reader.GetInt32("Id"),
                            Imagen = reader.IsDBNull(3) ? null : reader.GetString(1),
                            Caracteristica = reader.GetString("caracteristica"),
                            Nombre = reader.GetString("nombre"),
                            Vida = reader.GetInt32("vida"),
                        };
                        res.Add(e);
                    }

                }
            }
            return res;
        }
    }
    }
