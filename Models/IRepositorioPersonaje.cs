using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace juegoCartas_net.Models
{
    public interface IRepositorioPersonaje : IRepositorio<Personaje>
    {
        public Personaje ObtenerPorParte(int caraId, int cabezaId, int cuerpoId);
        public IList<Personaje> ObtenerLista(int paginaNro = 1, int tamPagina = 10);
        public int ObtenerCantidad();
        public IList<Personaje> BuscarPorNombre(string nombre);
    }
}
