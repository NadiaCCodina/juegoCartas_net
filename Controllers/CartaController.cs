using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using juegoCartas_net.Models;


namespace juegoCartas_net.Controllers
{

    public class CartaController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioCarta repositorio;
        private readonly IRepositorioPersonaje repoPersonaje;

        private readonly IRepositorioMazo repoMazo;


        public CartaController(IConfiguration configuration, IWebHostEnvironment environment,
        IRepositorioCarta repositorio, IRepositorioMazo repoMazo, IRepositorioPersonaje repoPersonaje)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repoMazo = repoMazo;
            this.repoPersonaje = repoPersonaje;

        }

        public ActionResult Create()
        {

            return View();
        }


        // POST: Cabeza/Create
         [HttpPost]
        [ValidateAntiForgeryToken]
   
        [HttpPost]
        public ActionResult CrearCarta(int personajeId)
        {

            try
            {
                Console.WriteLine("Método CrearCarta ejecutado.");

                string email = User.Identity.Name;
                Console.WriteLine("Método CrearCarta EMAIL." + email);

            
                Mazo mazoId = repoMazo.ObtenerPorEmailUsuario(email);

                Carta c = new Carta
                {
                    PersonajeId = personajeId,

                    MazoId = mazoId.Id,

                };
                int puntosPersonaje = repoPersonaje.ObtenerPorId(personajeId).PuntosHabilidad;
                var monedasRestantes = mazoId.PuntosHabilidad - puntosPersonaje;
               if (monedasRestantes >= 0)
                {
                    Mazo m = new Mazo
                    {
                        Id = mazoId.Id,
                        PuntosHabilidad = monedasRestantes

                    };
                    repoMazo.Modificacion(m);
                    repositorio.Alta(c);
                    TempData["Mensaje"] = "Carta Creada!";
                   return RedirectToAction("CreateCartaPersonaje", "Personaje");
                }
                else
                {
                       TempData["Mensaje"] = "Las monedas no alcanzan!";
                      return RedirectToAction("CreateCartaPersonaje", "Personaje");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear la Carta: " + ex.Message);
                return View();
            }
        }

    }
}
