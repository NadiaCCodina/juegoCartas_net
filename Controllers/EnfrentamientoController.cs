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

        public ActionResult Create(Enfrentamiento e)
        {
            if (!ModelState.IsValid)
            {
                return View(e);
            }

            try
            {

                var random = repoUsuario.ObtenerRandom();
                var usuarioRandom = repoUsuario.ObtenerPorId(random.Id);
                Enfrentamiento enf = new Enfrentamiento
                {
                    RetadorId = e.RetadorId,
                    ContrincanteId = usuarioRandom.Id,


                };
                repositorio.Alta(enf);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el cuerpo: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public IActionResult Partida(List<int> cartasSeleccionadas)
        {
            try
            {
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
           
            ViewBag.CartasRetador = cartasRetador;
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