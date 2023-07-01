using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet_domain.Entities;
using myfinance_web_dotnet_service.Interfaces;

namespace myfinance_web_dotnet.Controllers;

public class TransacaoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITransacaoService _transacaoService;
    private readonly IPlanoContaService _planoContaService;

    public TransacaoController(ILogger<HomeController> logger, ITransacaoService transacaoService, IPlanoContaService planoContaService)
    {
        _logger = logger;
        _transacaoService = transacaoService;
        _transacaoService = transacaoService;
        _planoContaService = planoContaService;
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
        var listaPlanoContas = new SelectList(_planoContaService.ListarRegistros(), "Id", "Descricao");

        var transacao = new TransacaoModel()
        {
            Data = DateTime.Now,
            ListaPlanoContas = listaPlanoContas,
        };

        if (Id != null)
        {
            var transacaoStored = _transacaoService.RetornarRegistro((int)Id);

            transacao.Id = transacaoStored.Id;
            transacao.Historico = transacaoStored.Historico;
            transacao.Data = transacaoStored.Data;
            transacao.Valor = transacaoStored.Valor;
            transacao.PlanoContaId = transacaoStored.PlanoContaId;
        }

        return View(transacao);
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
