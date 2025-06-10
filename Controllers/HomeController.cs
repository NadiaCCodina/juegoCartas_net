using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using juegoCartas_net.Models;

namespace juegoCartas_net.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
      private readonly IRepositorioCarta repoCarta;

    public HomeController(ILogger<HomeController> logger, IRepositorioCarta repoCarta)
    {
        _logger = logger;
        this.repoCarta = repoCarta;
    }

    public IActionResult Index()
    {
        var cartas = repoCarta.ObtenerPorIdUsuario(12);
        ViewBag.Cartas = cartas;
          Console.WriteLine("Retadorid" + cartas.Count);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
