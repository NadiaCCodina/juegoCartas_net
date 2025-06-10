using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using juegoCartas_net.Models;

namespace juegoCartas_net.Controllers
{

    public class EnfrentamientoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioEnfrentamiento repositorio;
        private readonly IRepositorioPersonaje repoPersonaje;
        private readonly IRepositorioCarta repoCarta;
        private readonly IRepositorioUsuario repoUsuario;
        private readonly IRepositorioMazo repoMazo;


        public EnfrentamientoController(IConfiguration configuration, IWebHostEnvironment environment,
        IRepositorioEnfrentamiento repositorio, IRepositorioMazo repoMazo, IRepositorioPersonaje repoPersonaje,
         IRepositorioCarta repoCarta, IRepositorioUsuario repoUsuario)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repoMazo = repoMazo;
            this.repoPersonaje = repoPersonaje;
            this.repoCarta = repoCarta;
            this.repoUsuario = repoUsuario;


        }

        public ActionResult Cartas()
        {
            int usuarioActualId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Cartas = repoCarta.ObtenerPorIdUsuario(usuarioActualId);
            //var cartas = repoCarta.ObtenerPorIdUsuario(8);
            var usuario = repoUsuario.ObtenerPorId(usuarioActualId);
            return View(usuario);
        }

        public ActionResult Create(int retadorId, int contrincanteId, List<int> cartasContrincanteIds, List<int> cartasRetadorIds)
        {


            // try
            // {
            List<Carta> cartasRetador = new List<Carta>();
            foreach (int id in cartasRetadorIds)
            {
                var carta = repoCarta.ObtenerPorId(id);
                if (carta != null)
                {
                    cartasRetador.Add(carta);
                }
            }
            List<Carta> cartasContrincante = new List<Carta>();
            foreach (int id in cartasContrincanteIds)
            {
                var carta = repoCarta.ObtenerPorId(id);
                if (carta != null)
                {
                    cartasContrincante.Add(carta);
                }
            }

            var primerCartaRetador = cartasRetador.First();
            var primerCartaContrincante = cartasContrincante.First();
            var vidaContrincante1 = primerCartaContrincante.Vida - primerCartaRetador.Ataque;
            var vidaRetador1 = primerCartaRetador.Vida - primerCartaContrincante.Ataque;
            Console.WriteLine("Retadorid" + retadorId);
            Console.WriteLine("Vida Retador" + primerCartaRetador.Vida);
            Console.WriteLine("Vida Contrincante" + primerCartaContrincante.Vida);

            var segundaCartaRetador = cartasRetador[1];
            var segundaCartaContrincante = cartasContrincante[1];
            var vidaContrincante2 = segundaCartaContrincante.Vida - segundaCartaRetador.Ataque;
            var vidaRetador2 = segundaCartaRetador.Vida - segundaCartaContrincante.Ataque;
            var tercerCartaRetador = cartasRetador[2];
            var tercerCartaContrincante = cartasContrincante[2];
            var vidaContrincante3 = tercerCartaContrincante.Vida - tercerCartaRetador.Ataque;
            var vidaRetador3 = tercerCartaRetador.Vida - tercerCartaContrincante.Ataque;
            var totalRetador = vidaRetador1 + vidaRetador2 + vidaRetador3;
            var totalContrincante = vidaContrincante1 + vidaContrincante2 + vidaContrincante3;
            int resultado = 0;
            int ganadorId = 0;
            Console.WriteLine("Vida Retador desp" + totalRetador);
            Console.WriteLine("Vida Contrincantedesp" + totalContrincante);
            Usuario ganador = null;
            List<Carta> CartasGanador = new List<Carta>();
            if (totalContrincante < totalRetador)
            {
                resultado = 1;
                ganador = repoUsuario.ObtenerPorId(retadorId);
                CartasGanador = cartasRetador;
                ganadorId = ganador.Id;
            }
            else if (totalContrincante == totalRetador)
            {
                resultado = 2;
            }
            else
            {
                resultado = 3;
                ganador = repoUsuario.ObtenerPorId(contrincanteId);
                CartasGanador = cartasContrincante;
                ganadorId = ganador.Id;
            }
            Console.WriteLine("Retadorid" + retadorId);
            Enfrentamiento enf = new Enfrentamiento
            {
                RetadorId = retadorId,
                ContrincanteId = contrincanteId,
                Resultado = resultado,
            };

            if (ganadorId > 0 && ganadorId == retadorId)
            {
                var mazo = repoMazo.ObtenerPorId(ganadorId);
                var puntos = mazo.PuntosHabilidad + 1;

                Mazo m = new Mazo
                {
                    Id = mazo.Id,
                    UsuarioId = ganadorId,
                    PuntosHabilidad = puntos,

                };
                Console.WriteLine("Puntos Retador:" + puntos);
                repoMazo.Modificacion(m);
            }


            repositorio.Alta(enf);
            Console.WriteLine("CARTAS RETADOR:");
            foreach (var carta in cartasRetador)
            {
                Console.WriteLine($"ID: {carta.Id}, Nombre: {carta.PersonajeNombre}");
            }
            Console.WriteLine("Total cartas: " + cartasRetador.Count);



            ViewBag.CartasRetador = cartasRetador;
            ViewBag.CartasContrincante = cartasContrincante;
            ViewBag.CartasRetador = cartasRetador;
            ViewBag.RetadorId = retadorId;
            ViewBag.ContrincanteId = contrincanteId;
            ViewBag.Ganador = ganador;

            ViewBag.CartasGanador = CartasGanador;
            ViewBag.Ganador = ganador;
            return Json(new { ganadorId });
            // }
            // catch (Exception ex)
            // {
            //     ModelState.AddModelError("", "Error al crear el cuerpo: " + ex.Message);
            //     return RedirectToAction(nameof(Index));
            // }
        }

   public IActionResult indexPorUsuario()
        {
            var resultado = repositorio.ObtenerResultadosJson(12);
            return Json(new
            {
                resultadoJson = resultado,

            });
        }

        [HttpPost]
        public IActionResult Partida(List<int> cartasSeleccionadas, int retadorId)
        {
            try
            {
                Usuario retador = repoUsuario.ObtenerPorId(retadorId);
                List<Carta> cartasRetador = new List<Carta>();

                foreach (int id in cartasSeleccionadas)
                {
                    var carta = repoCarta.ObtenerPorId(id);
                    if (carta != null)
                    {
                        cartasRetador.Add(carta);
                    }
                }
                int randomId;
                List<Carta> cartasContrincante;
                Usuario usuarioRandom;
                int intentos = 0;
                int maxIntentos = 10;

                do
                {
                    randomId = repoUsuario.ObtenerRandom();
                    usuarioRandom = repoUsuario.ObtenerPorId(randomId);
                    cartasContrincante = repoCarta.ObtenerPorIdUsuarioRandom(usuarioRandom.Id).ToList();
                    intentos++;
                } while ((cartasContrincante == null || cartasContrincante.Count < 3) && intentos < maxIntentos);

                if (cartasContrincante == null || cartasContrincante.Count < 3)
                {
                    TempData["Error"] = "No se encontró un contrincante con suficientes cartas.";
                    return RedirectToAction("Cartas");
                }
                var firstElement = cartasRetador.First();
                ViewBag.CartasRetador = cartasRetador;
                ViewBag.Retador = retador;
                ViewBag.UsuarioRandom = usuarioRandom;
                ViewBag.CartasContrincante = cartasContrincante;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error  " + ex.Message);
                return RedirectToAction(nameof(Index));
            }


        }
    }
}