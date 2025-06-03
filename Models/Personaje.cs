using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
    public class Personaje
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int CaraId { get; set; }
        [ForeignKey("CaraId")]
        public virtual Cara Cara { get; set; }

        public int CuerpoId { get; set; }
        [ForeignKey("CuerpoId")]


        public int CabezaId { get; set; }
        [ForeignKey("CabezaId")]
        public virtual Cabeza Cabeza { get; set; }

        public string? Imagen { get; set; }
        public IFormFile? ImagenFile { get; set; }
        public int Vida { get; set; }
        public int Ataque { get; set; }
        public int Tipo { get; set; }

         public int PuntosHabilidad { get; set; }
        

    }
}