using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using juegoCartas_net.Models;

namespace juegoCartas_net.Controllers

{
    [Authorize(Policy = "Administrador")]
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

        public ActionResult Eliminar(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }
        }
        [HttpPost]
        public ActionResult Eliminar(Cabeza entidad)
        {
            try
            {
                repositorio.Baja(entidad.Id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Create()
        {

            return View();
        }


        // POST: Cabeza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        [HttpPost]
        public ActionResult Create(Cabeza c)
        {
            if (!ModelState.IsValid)
            {
                return View(c);
            }

            // try
            // {
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
            // }
            // catch (Exception ex)
            // {
            //     ModelState.AddModelError("", "Error al crear la Cabeza: " + ex.Message);
            //     return View(c);
            // }
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
                ModelState.AddModelError("", "Error al editar la Cabeza: " + ex.Message);
                return View(id);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

    public IActionResult Edit(int id, Cabeza entidad)
{
    try
    {
        var c = repositorio.ObtenerPorId(id);

        if (c == null)
        {
            return NotFound();
        }

        c.Nombre = entidad.Nombre;
        c.Caracteristica = entidad.Caracteristica;
        c.Ataque = entidad.Ataque;

        if (entidad.ImagenFile != null && entidad.ImagenFile.Length > 0)
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "imagenes");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = "cabeza_" + id + Path.GetExtension(entidad.ImagenFile.FileName);
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
        ModelState.AddModelError("", "Error al modificar la Cabeza: " + ex.Message);
        return View(entidad);
    }
}


    }
}
