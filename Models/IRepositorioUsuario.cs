using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {

        public Usuario ObtenerPorEmail(string email);
        public Usuario ObtenerRandom();
    }
}
