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

        private readonly IRepositorioMazo repoMazo;


        public CartaController(IConfiguration configuration, IWebHostEnvironment environment,
        IRepositorioCarta repositorio, IRepositorioMazo repoMazo )
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repoMazo = repoMazo;

        }

        public ActionResult Create()
        {

            return View();
        }
        

        	// POST: Cabeza/Create
		// [HttpPost]
		 [ValidateAntiForgeryToken]
		// [Authorize(Policy = "Administrador")]
		[HttpPost]
public ActionResult CrearCarta(int personajeId)
{
            // if (!ModelState.IsValid)
            // {
            //     return View(c);
            // }

            // try
            // {
             Console.WriteLine("Método CrearCarta ejecutado.");
             
            string email = User.Identity.Name;
            Console.WriteLine(email);
        
           //int usuarioActualId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // Console.WriteLine("UsuarioActualId: " + usuarioActualId);
            Mazo mazoId = repoMazo.ObtenerPorEmailUsuario(email);

            Carta c = new Carta
            {
                PersonajeId = personajeId,
               
                MazoId = mazoId.Id,
                
            };
    	
        repositorio.Alta(c);
        return RedirectToAction(nameof(Index), "Home");
    // }
    // catch (Exception ex)
    // {
    //     ModelState.AddModelError("", "Error al crear la Carta: " + ex.Message);
    //     return View();
    // }
}

}
}
