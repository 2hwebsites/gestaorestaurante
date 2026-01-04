using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EstoqueLocal.Web.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    public HomeController(IConfiguration configuration) => _configuration = configuration;

    public IActionResult Index()
    {
        ViewBag.LogoPath = _configuration["Branding:LogoPath"] ?? "/img/logo.png";
        ViewBag.AppName = _configuration["Branding:AppName"] ?? "Gestão de Estoque";
        return View();
    }
}
