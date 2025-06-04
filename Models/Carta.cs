using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
    [Table("Carta")]
    public class Carta
    {

        public int Id { get; set; }
        public int PersonajeId { get; set; }

        public int MazoId { get; set; }

        public int? Nivel { get; set; }

        public string? PersonajeNombre { get; set; }
        public int? PuntosHabilidad { get; set; }
        public string? Imagen { get; set; }
        public IFormFile? ImagenFile { get; set; }  
        public int Vida { get; set; }
        public int Ataque { get; set; }
        public int Tipo { get; set; }




    }
}