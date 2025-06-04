using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
    public class Enfrentamiento
    {
        public int Id { get; set; }

        public int RetadorId { get; set; }
        [ForeignKey("RetadorId")]

        public int ContrincanteId { get; set; }
        [ForeignKey("ContrincanteId")]

        public int Resultado { get; set; }
        
        public DateTime Fecha  { get; set; }

    }
}