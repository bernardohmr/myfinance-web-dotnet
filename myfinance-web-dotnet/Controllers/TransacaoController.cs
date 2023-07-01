using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet_domain.Entities;
using myfinance_web_dotnet_service.Interfaces;

namespace myfinance_web_dotnet.Controllers;

public class TransacaoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITransacaoService _transacaoService;

    public TransacaoController(ILogger<HomeController> logger, ITransacaoService transacaoService)
    {
        _logger = logger;
        _transacaoService = transacaoService;
    }

    public IActionResult Index()
    {
        var listaTransacoes = _transacaoService.ListarRegistros();
        List<TransacaoModel> listaTransacaoModel = new List<TransacaoModel>();

        foreach (var item in listaTransacoes) {
            var itemTransacao = new TransacaoModel() {
                Id = item.Id,
                Historico = item.Historico,
                Data = item.Data,
                Valor = item.Valor,
                PlanoContaId = item.PlanoContaId,
                Tipo = item.PlanoConta.Tipo,
            };

            listaTransacaoModel.Add(itemTransacao);
        }

        ViewBag.ListaTransacoes = listaTransacaoModel;

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
            var transacao = _transacaoService.RetornarRegistro((int)Id);
            var transacaoModel = new TransacaoModel()
            {
                Id = transacao.Id,
                Historico = transacao.Historico,
                Data = transacao.Data,
                Valor = transacao.Valor,
                PlanoContaId = transacao.PlanoContaId,
            };

            return View(transacaoModel);
        }
        else
        {
            return View();
        }
    }

    [HttpPost]
    public IActionResult Cadastrar(TransacaoModel transacaoModel)
    {
        var transacao = new Transacao()
        {
            Id = transacaoModel.Id,
            Historico = transacaoModel.Historico,
            Data = transacaoModel.Data,
            Valor = transacaoModel.Valor,
            PlanoContaId = transacaoModel.PlanoContaId,
        };

        _transacaoService.Cadastrar(transacao);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Excluir(int Id)
    {
        _transacaoService.Excluir(Id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
