using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
  [Table("Cabeza")]
  public class Cabeza
  {

    public int Id { get; set; }

    public string? Imagen { get; set; }
    public IFormFile? ImagenFile { get; set; }
    public string Caracteristica { get; set; }
    public string Nombre { get; set; }
    public int Ataque { get; set; }


    }
}