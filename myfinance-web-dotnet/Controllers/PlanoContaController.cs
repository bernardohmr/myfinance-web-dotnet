using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet_domain.Entities;
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

    public IActionResult Cadastrar(int? Id)
    {
        if (Id != null)
        {
            var planoConta = _planoContaService.RetornarRegistro((int)Id);
            var planoContaModel = new PlanoContaModel()
            {
                Id = planoConta.Id,
                Descricao = planoConta.Descricao,
                Tipo = planoConta.Tipo,
            };

            return View(planoContaModel);
        }
        else
        {
            return View();
        }
    }

    [HttpPost]
    public IActionResult Cadastrar(PlanoContaModel planoContaModel)
    {
        var planoConta = new PlanoConta()
        {
            Id = planoContaModel.Id,
            Descricao = planoContaModel.Descricao,
            Tipo = planoContaModel.Tipo,
        };

        _planoContaService.Cadastrar(planoConta);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Excluir(int Id)
    {
        _planoContaService.Excluir(Id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
