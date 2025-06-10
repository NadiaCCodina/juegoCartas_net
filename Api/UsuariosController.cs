using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;


using juegoCartas_net.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace juegoCartas_net.Api
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UsuariosController : Controller
	{
		private readonly IConfiguration configuration;
		private readonly IWebHostEnvironment environment;
		private readonly IRepositorioUsuario repositorio;
		private readonly IRepositorioCarta repoCarta;
		private readonly IRepositorioEnfrentamiento repoEnfrentamiento;


		public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment,
		 IRepositorioUsuario repositorio, IRepositorioCarta repoCarta, IRepositorioEnfrentamiento repoEnfrentamiento)
		{
			this.configuration = configuration;
			this.environment = environment;
			this.repositorio = repositorio;
			this.repoCarta = repoCarta;
			this.repoEnfrentamiento = repoEnfrentamiento;


		}



		[HttpPost("login")]
		[AllowAnonymous]
		public IActionResult Login([FromForm] LoginView login)
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

					var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
					return Json(new { access_token = stringToken });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[Route("resultadosEnfrentamientos")]
		public IActionResult ResultadosPorUsuario()
		{
			var usuarioActual = repositorio.ObtenerPorEmail(User.Identity.Name);

			var resultado = repoEnfrentamiento.ObtenerResultadosJson(usuarioActual.Id);
			return Json(new
			{
				resultadoJson = resultado,

			});
		}
	}
}

