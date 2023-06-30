using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet_service.Interfaces;

namespace myfinance_web_dotnet.Controllers;

public class PlanoContaController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPlanoContaService _planoContaService;

    public PlanoContaController(ILogger<HomeController> logger, IPlanoContaService planoContaService)
    {
        _logger = logger;
        _planoContaService = planoContaService;
    }

    [HttpGet]
    [Route("Index")]
    public IActionResult Index()
    {
        var listaPlanoContas = _planoContaService.ListarRegistros();
        List<PlanoContaModel> listaPlanoContaModel = new List<PlanoContaModel>();

        foreach (var item in listaPlanoContas) {
            var itemPlanoConta = new PlanoContaModel() {
                Id = item.Id,
                Descricao = item.Descricao,
                Tipo = item.Tipo
            };

            listaPlanoContaModel.Add(itemPlanoConta);
        }

        ViewBag.ListaPlanoConta = listaPlanoContaModel;

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
