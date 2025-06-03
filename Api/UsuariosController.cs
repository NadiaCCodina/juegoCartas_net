using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;


using juegoCartas_net.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace juegoCartas_net.Api
{
    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioUsuario repositorio;
        private readonly IRepositorioCarta repoCarta;
    

        public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioUsuario repositorio, IRepositorioCarta repoCarta)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.repoCarta = repoCarta;
         
            
        }
        // [Route("[controller]/Index/")]
        // // [Authorize(Policy = "Administrador")]
        // public ActionResult PerfilCartas()
        // {
        //     ViewBag.Cartas = repoCarta.ObtenerPorIdUsuario(8);
        //     //var cartas = repoCarta.ObtenerPorIdUsuario(8);
        //     var usuario = repositorio.ObtenerPorId(8);
        //     return View(usuario);
        // }

        // // public ActionResult Perfil()
        // // {
        // //     var usuarios = repositorio.ObtenerTodos();
        // //     return View(usuarios);
        // // }
        // // GET: Usuarios/Create
        // // [Authorize(Policy = "Administrador")]
        // public ActionResult Create()
        // {
        //     ViewBag.Roles = Usuario.ObtenerRoles();
        //     return View(new Usuario());
        // }

        // // POST: Usuarios/Create
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // // [Authorize(Policy = "Administrador")]
        // public ActionResult Create(Usuario u)
        // {
        //     if (!ModelState.IsValid)
        //         return View();
        //     try
        //     {
        //         string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //                         password: u.Clave,
        //                         salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
        //                         prf: KeyDerivationPrf.HMACSHA1,
        //                         iterationCount: 1000,
        //                         numBytesRequested: 256 / 8));
        //         u.Clave = hashed;

        //         var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
        //         int res = repositorio.Alta(u);
        //         if (u.AvatarFile != null && u.Id > 0)
        //         {
        //             string wwwPath = environment.WebRootPath;
        //             string path = Path.Combine(wwwPath, "Uploads");
        //             if (!Directory.Exists(path))
        //             {
        //                 Directory.CreateDirectory(path);
        //             }
        //             //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
        //             string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
        //             string pathCompleto = Path.Combine(path, fileName);
        //             u.Avatar = Path.Combine("/Uploads", fileName);
        //             // Esta operación guarda la foto en memoria en la ruta que necesitamos
        //             using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
        //             {
        //                 u.AvatarFile.CopyTo(stream);
        //             }
        //             repositorio.Modificacion(u);
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     catch (Exception ex)
        //     {
        //         ViewBag.Roles = Usuario.ObtenerRoles();
        //         return View();
        //     }
        // }
        // // GET: Usuarios/Edit/5
        // // [Authorize]
        // public ActionResult Perfil()
        // {
        //     ViewData["Title"] = "Mi perfil";
        //     var u = repositorio.ObtenerPorEmail(User.Identity.Name);
        //     ViewBag.Roles = Usuario.ObtenerRoles();
        //     return View("Edit", u);
        // }

        // // GET: Usuarios/Edit/5
        // // [Authorize(Policy = "Administrador")]
        // public ActionResult Edit(int id)
        // {
        //     ViewData["Title"] = "Editar usuario";
        //     var u = repositorio.ObtenerPorId(id);
        //     ViewBag.Roles = Usuario.ObtenerRoles();
        //     return View(u);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // // [Authorize]
        // public ActionResult Edit(int id, Usuario u)
        // {
        //     var vista = nameof(Edit);//de que vista provengo
        //     try
        //     {
        //         if (!User.IsInRole("Administrador"))//no soy admin
        //         {
        //             vista = nameof(Perfil);//solo puedo ver mi perfil
        //             var usuarioActual = repositorio.ObtenerPorEmail(User.Identity.Name);
        //             if (usuarioActual.Id != id)
        //             {//si no es admin, solo puede modificarse él mismo
        //                 return RedirectToAction(nameof(Index), "Home");
        //             }
        //             else
        //             {

        //                 usuarioActual.Nombre = u.Nombre;



        //                 if (!string.IsNullOrWhiteSpace(u.Clave))
        //                 {
        //                     string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //                                     password: u.Clave,
        //                                     salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
        //                                     prf: KeyDerivationPrf.HMACSHA1,
        //                                     iterationCount: 1000,
        //                                     numBytesRequested: 256 / 8));


        //                     usuarioActual.Clave = hashed;
        //                 }


        //                 if (u.AvatarFile != null && u.Id > 0)
        //                 {
        //                     string wwwPath = environment.WebRootPath;
        //                     string path = Path.Combine(wwwPath, "Uploads");
        //                     if (!Directory.Exists(path))
        //                     {
        //                         Directory.CreateDirectory(path);
        //                     }
        //                     //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
        //                     string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
        //                     string pathCompleto = Path.Combine(path, fileName);
        //                     usuarioActual.Avatar = Path.Combine("/Uploads", fileName);
        //                     // Esta operación guarda la foto en memoria en la ruta que necesitamos
        //                     using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
        //                     {
        //                         u.AvatarFile.CopyTo(stream);
        //                     }
        //                 }
        //                 repositorio.Modificacion(usuarioActual);
        //                 TempData["Mensaje"] = "Datos guardados correctamente";
        //                 return RedirectToAction(nameof(PerfilCartas), "Usuario");

        //             }
        //         }
        //         else
        //         {
        //             u.Id = id;
        //             if (!string.IsNullOrWhiteSpace(u.Clave))
        //             {
        //                 string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //                                 password: u.Clave,
        //                                 salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
        //                                 prf: KeyDerivationPrf.HMACSHA1,
        //                                 iterationCount: 1000,
        //                                 numBytesRequested: 256 / 8));


        //                 u.Clave = hashed;
        //             }
        //             repositorio.Modificacion(u);
        //             TempData["Mensaje"] = "Datos guardados correctamente";
        //             return RedirectToAction(nameof(Index), "Home");

        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         throw;
        //     }
        // }


        
 
      	[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromForm] LoginView login)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: login.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
			    var p = repositorio.ObtenerPorEmail(login.Usuario);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
					System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("FullName", p.Nombre),
						new Claim(ClaimTypes.Role, p.RolNombre),
					};

					var token = new JwtSecurityToken(
						issuer: configuration["TokenAuthentication:Issuer"],
						audience: configuration["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


        [Route("salir", Name = "logoutApi")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Usuarios/Delete/5
        // [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)

        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id, Usuario usuario)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}

