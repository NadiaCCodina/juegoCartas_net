using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
    public interface IRepositorioMazo : IRepositorio<Mazo>
    {
        public Mazo ObtenerPorEmailUsuario(string email);
    }
    
}
