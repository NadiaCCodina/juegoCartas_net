using juegoCartas_net.Models;
using Microsoft.AspNetCore.Mvc;

namespace juegoCartas_net.Controllers
{

    public class CabezaController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioCabeza repositorio;

        public CabezaController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioCabeza repositorio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;

        }


        public ActionResult Index()
        {
            var cuerpos = repositorio.ObtenerTodos();
            return View(cuerpos);
        }


        public ActionResult Create()
        {

            return View();
        }


        // POST: Cabeza/Create
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // [Authorize(Policy = "Administrador")]
        [HttpPost]
        public ActionResult Create(Cabeza c)
        {
            if (!ModelState.IsValid)
            {
                return View(c);
            }

            try
            {
                if (c.ImagenFile != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileName = "cabeza_" + Guid.NewGuid().ToString() + Path.GetExtension(c.ImagenFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    c.Imagen = Path.Combine("/Uploads", fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        c.ImagenFile.CopyTo(stream);
                    }
                }
                else
                {
                    ModelState.AddModelError("ImagenFile", "Debe subir una imagen.");
                    return View(c);
                }

                repositorio.Alta(c);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear la Cabeza: " + ex.Message);
                return View(c);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);//pasa el modelo a la vista
            }
            catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }
        }
        	[HttpPost]
		[ValidateAntiForgeryToken]
		
        public ActionResult Edit(int id, Cabeza entidad)
		{
			
			Cabeza c = null;
			try
			{
				c = repositorio.ObtenerPorId(id);
		
				c.Nombre = entidad.Nombre;
				c.Caracteristica = entidad.Caracteristica;
                c.Ataque = entidad.Ataque;
				c.ImagenFile = entidad.ImagenFile;			
				repositorio.Modificacion(c);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				throw;
			}
		}

    }
}
