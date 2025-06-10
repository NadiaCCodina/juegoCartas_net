using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using juegoCartas_net.Models;

namespace juegoCartas_net.Controllers
{

    public class PersonajeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioPersonaje repositorio;
        private readonly IRepositorioCuerpo repoCuerpo;
        private readonly IRepositorioCabeza repoCabeza;
        private readonly IRepositorioCara repoCara;
        private readonly IRepositorioUsuario repoUsuario;
        private readonly IRepositorioMazo repoMazo;
        public PersonajeController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioPersonaje repositorio, IRepositorioCuerpo repoCuerpo,
        IRepositorioCabeza repoCabeza, IRepositorioCara repoCara, IRepositorioUsuario repoUsuario, IRepositorioMazo repoMazo

)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repoCuerpo = repoCuerpo;
            this.repoCabeza = repoCabeza;
            this.repoCara = repoCara;
            this.repoUsuario = repoUsuario;
            this.repoMazo = repoMazo;



        }
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var personaje = repositorio.ObtenerTodos();
            return View(personaje);
        }


        [Authorize(Policy = "Administrador")]
        [Route("[controller]/Lista")]
        public ActionResult Lista(int pagina = 1)
        {
            try
            {
                var tamaño = 5;
                var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
                ViewBag.Pagina = pagina;
                var total = repositorio.ObtenerCantidad();
                ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño + 1;
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View("Index", lista);

            }
            catch (Exception ex)
            {// Poner breakpoints para detectar errores
                throw;
            }
        }
  [Authorize(Policy = "Administrador")]
        [Route("[controller]/buscarporpersonaje")]
        public ActionResult BuscarPorPersonaje(int id )
        {
            try
            {
               
                var personaje = repositorio.ObtenerPorId(id);
               
               
              
                return View("Edit", personaje);

            }
            catch (Exception ex)
            {// Poner breakpoints para detectar errores
                throw;
            }
        }
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            var personaje = repositorio.ObtenerTodos();
            var cara = repoCara.ObtenerTodos();
            var cuerpo = repoCuerpo.ObtenerTodos();
            var cabeza = repoCabeza.ObtenerTodos();
            return Json(new
            {
                carajson = cara,
                cuerpojson = cuerpo,
                cabezajson = cabeza,
                personajejson = personaje,

            });
        }

        public ActionResult CreateCartaPersonaje()
        {
            string email = User.Identity.Name;
            ViewBag.mazo = repoMazo.ObtenerPorEmailUsuario(email);
            return View();
        }

        [HttpGet]
        //Obtengo las partes de los personajes
        public IActionResult ObtenerPartes()
        {
            var personaje = repositorio.ObtenerTodos();
            var cara = repoCara.ObtenerTodos();
            var cuerpo = repoCuerpo.ObtenerTodos();
            var cabeza = repoCabeza.ObtenerTodos();
            return Json(new
            {
                carajson = cara,
                cuerpojson = cuerpo,
                cabezajson = cabeza,
                personajejson = personaje,

            });
        }
        //Cambio el personaje segun la parte Elegida
        public IActionResult cambio(int cara, int cabeza, int cuerpo)
        {
            var personaje = repositorio.ObtenerPorParte(cara, cabeza, cuerpo);

            return Json(new
            {

                personajejson = personaje,

            });
        }
        [HttpGet]
        //Obtengo el personaje por defecto cuando cargo la pagina
        public IActionResult ObtenerPersonaje()
        {
            var personaje = repositorio.ObtenerPorParte(1, 1, 1);
            return Json(new
            {
                personajejson = personaje,

            });
        }



        [Authorize(Policy = "Administrador")]
        //Carga la pagina de crear diseño de personaje
        public ActionResult CreateDiseño()
        {
            ViewBag.Caras = repoCara.ObtenerTodos();
            ViewBag.Cabezas = repoCabeza.ObtenerTodos();
            ViewBag.Cuerpos = repoCuerpo.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        //crea el diseño de personaje
        public ActionResult CreatePersonajeNuevo(Personaje c)
        {
            int puntoHabilidad = 0;
            // if (!ModelState.IsValid)
            // {
            //     return View(c);
            // }

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

                    string fileName = "personaje_" + Guid.NewGuid().ToString() + Path.GetExtension(c.ImagenFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    c.Imagen = Path.Combine("/Uploads", fileName).Replace("\\", "/");

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        c.ImagenFile.CopyTo(stream);
                    }
                }
                else
                {
                    ModelState.AddModelError("ImagenFile", "Debe subir una imagen.");
                    return RedirectToAction(nameof(Create));
                }
                int tipo = repoCara.ObtenerPorId(c.CaraId).Tipo;
                int ataque = repoCabeza.ObtenerPorId(c.CabezaId).Ataque;
                int vida = repoCuerpo.ObtenerPorId(c.CuerpoId).Vida;
                puntoHabilidad = ataque + vida + tipo;
                c.PuntosHabilidad = puntoHabilidad;
                repositorio.Alta(c);
                return RedirectToAction(nameof(Lista));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el personaje: " + ex.Message);
                return RedirectToAction(nameof(CreateCartaPersonaje));
            }
        }


        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {

            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id, Personaje entidad)
        {

            Personaje c = null;
            try
            {
                int puntoHabilidad = 0;
                c = repositorio.ObtenerPorId(id);

                c.Nombre = entidad.Nombre;
                c.CaraId = entidad.CaraId;
                c.CabezaId = entidad.CabezaId;
                c.CuerpoId = entidad.CuerpoId;
                c.ImagenFile = entidad.ImagenFile;
                int tipo = repoCara.ObtenerPorId(c.CaraId).Tipo;
                int ataque = repoCabeza.ObtenerPorId(c.CabezaId).Ataque;
                int vida = repoCuerpo.ObtenerPorId(c.CuerpoId).Vida;
                puntoHabilidad = ataque + vida + tipo;
                c.PuntosHabilidad = puntoHabilidad;
                repositorio.Modificacion(c);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error al editar: " + ex.Message);
                return RedirectToAction(nameof(CreateCartaPersonaje));
            }
        }
        //Para el select2 de index
        [Route("[controller]/Buscarjson/{q}", Name = "BuscarPersonajeJson")]
        public ActionResult Buscarjson(string q)
        {
            try
            {
                var res = repositorio.BuscarPorNombre(q);


                return Json(new { Datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
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
                ModelState.AddModelError("", "Error " + ex.Message);
                return RedirectToAction(nameof(Lista));
            }
        }
        [HttpPost]
        public ActionResult Eliminar(Cabeza entidad)
        {
            try
            {
                repositorio.Baja(entidad.Id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Lista));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al Eliminar: " + ex.Message);
                return RedirectToAction(nameof(Lista));
            }
        }
    }
}
