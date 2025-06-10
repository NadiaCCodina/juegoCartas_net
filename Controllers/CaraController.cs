using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using juegoCartas_net.Models;

namespace juegoCartas_net.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class CaraController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioCara repositorio;

        public CaraController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioCara repositorio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;

        }

        public ActionResult Index()
        {
            var cara = repositorio.ObtenerTodos();
            return View(cara);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpGet]
        public IActionResult ObtenerTodas()
        {
            var caras = repositorio.ObtenerTodos();
            return Json(caras);
        }

        // POST: Cabeza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        public ActionResult Create(Cara c)
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

                    string fileName = "cara_" + Guid.NewGuid().ToString() + Path.GetExtension(c.ImagenFile.FileName);
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
                ModelState.AddModelError("", "Error al crear la Cara: " + ex.Message);
                return View(c);
            }
        }
        public ActionResult Eliminar(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al cargar la vista " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public ActionResult Eliminar(Cara entidad)
        {
            try
            {
                repositorio.Baja(entidad.Id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar la Cara: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al cargar la vista " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int id, Cara entidad)
        {

            Cara c = null;
            try
            {
                c = repositorio.ObtenerPorId(id);

                c.Nombre = entidad.Nombre;
                c.Caracteristica = entidad.Caracteristica;
                c.Ataque = entidad.Ataque;
                c.Tipo = entidad.Tipo;
                if (entidad.ImagenFile != null && entidad.ImagenFile.Length > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "imagenes");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileName = "cara_" + id + Path.GetExtension(entidad.ImagenFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);

                    c.Imagen = Path.Combine("/imagenes", fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        entidad.ImagenFile.CopyTo(stream);
                    }
                }
                repositorio.Modificacion(c);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al editar la Cara: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
