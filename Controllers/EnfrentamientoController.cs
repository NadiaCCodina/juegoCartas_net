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
                    Console.WriteLine("Retadorid" +  retadorId);
                    Console.WriteLine("Vida Retador" +  primerCartaRetador.Vida);
                    Console.WriteLine("Vida Contrincante" + primerCartaContrincante.Vida);
                    Console.WriteLine("Vida Retador desp" + vidaRetador1);
                    Console.WriteLine("Vida Contrincantedesp" + vidaContrincante1);
                int resultado = 0;
                Usuario ganador = null;
                if (vidaContrincante1 < vidaRetador1)
                {
                    resultado = 1;
                    ganador = repoUsuario.ObtenerPorId(retadorId);
                }
                else if (vidaContrincante1 == vidaRetador1)
                {
                    resultado = 2;
                }
                else
                {
                    resultado = 3;
                     ganador = repoUsuario.ObtenerPorId(contrincanteId);
                }
  Console.WriteLine("Retadorid" +  retadorId);
                    Enfrentamiento enf = new Enfrentamiento
                    {
                        RetadorId = retadorId,
                        ContrincanteId = contrincanteId,
                        Resultado = resultado,

                    };

                repositorio.Alta(enf);
                ViewBag.Ganador = ganador;
                 return View("Resultado");
            // }
            // catch (Exception ex)
            // {
            //     ModelState.AddModelError("", "Error al crear el cuerpo: " + ex.Message);
            //     return RedirectToAction(nameof(Index));
            // }
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
                Usuario random;
                List<Carta> cartasContrincante;
                Usuario usuarioRandom;
                int intentos = 0;
                int maxIntentos = 10;

                do
                {
                    random = repoUsuario.ObtenerRandom();

                    usuarioRandom = repoUsuario.ObtenerPorId(random.Id);

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