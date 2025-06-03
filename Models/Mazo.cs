using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
    [Table("Mazo")]
    public class Mazo
    {

        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PuntosHabilidad { get; set; }


    }

}

