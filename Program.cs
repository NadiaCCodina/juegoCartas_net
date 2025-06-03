using juegoCartas_net.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositorioCuerpo, RepositorioCuerpo>();
builder.Services.AddScoped<IRepositorioCabeza, RepositorioCabeza>();
builder.Services.AddScoped<IRepositorioCara, RepositorioCara>();
builder.Services.AddScoped<IRepositorioPersonaje, RepositorioPersonaje>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioCarta, RepositorioCarta>();
builder.Services.AddScoped<IRepositorioMazo, RepositorioMazo>();

var configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
	options.LoginPath = "/Usuario/Login";
	options.LogoutPath = "/Usuario/Logout";
	options.AccessDeniedPath = "/Home/Restringido";
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = configuration["TokenAuthentication:Issuer"],
		ValidAudience = configuration["TokenAuthentication:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(
			System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]))
	};

	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
			var accessToken = context.Request.Query["access_token"];
			var path = context.HttpContext.Request.Path;

			if (!string.IsNullOrEmpty(accessToken) &&
				(path.StartsWithSegments("/chatsegurohub") ||
				 path.StartsWithSegments("/api/propietarios/reset") ||
				 path.StartsWithSegments("/api/propietarios/token")))
			{
				context.Token = accessToken;
			}
			return Task.CompletedTask;
		},
		OnTokenValidated = context =>
		{
			Console.WriteLine("Token válido para el usuario: " + context?.Principal?.Identity?.Name);
			return Task.CompletedTask;
		},
		OnAuthenticationFailed = context =>
		{
			Console.WriteLine("Error en la autenticación del token: " + context.Exception.Message);
			return Task.CompletedTask;
		}
	};
});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Administrador", policy =>
		policy.RequireRole("Administrador", "SuperAdministrador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
