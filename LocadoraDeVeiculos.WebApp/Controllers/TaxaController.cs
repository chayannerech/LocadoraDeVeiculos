using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class TaxaController(TaxaService servicoTaxa, AluguelService servicoAluguel, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoTaxa.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (!User.Identity!.IsAuthenticated)
            resultado = servicoTaxa.SelecionarTodos();

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        if (servicoTaxa.SemRegistros())
            ApresentarMensagemSemRegistros();

        var listarTaxaVm = mapeador.Map<IEnumerable<ListarTaxaViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarTaxaVm);
    }


    [Authorize(Roles = "Empresa, Funcionário")]
    public IActionResult Inserir() => View();
    [HttpPost]
    public IActionResult Inserir(InserirTaxaViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(inserirRegistroVm);

        var novoRegistro = mapeador.Map<Taxa>(inserirRegistroVm);

        if (servicoTaxa.ValidarRegistroRepetido(novoRegistro))
        {
            ApresentarMensagemRegistroExistente("Já existe um cadastro com esse nome");
            return View(inserirRegistroVm);
        }

        novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoTaxa.Inserir(novoRegistro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    [Authorize(Roles = "Empresa, Funcionário")]
    public IActionResult Editar(int id)
    {
        var resultado = servicoTaxa.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var editarRegistroVm = mapeador.Map<EditarTaxaViewModel>(registro);

        return View(editarRegistroVm);
    }    
    [HttpPost]
    public IActionResult Editar(EditarTaxaViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(editarRegistroVm);

        var registro = mapeador.Map<Taxa>(editarRegistroVm);

        if (servicoTaxa.ValidarRegistroRepetido(registro))
        {
            ApresentarMensagemRegistroExistente("Já existe um cadastro com esse nome");
            return View(editarRegistroVm);
        }

        var resultado = servicoTaxa.Editar(registro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    [Authorize(Roles = "Empresa, Funcionário")]
    public IActionResult Excluir(int id)
    {
        var resultado = servicoTaxa.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var detalhesTaxaViewModel = mapeador.Map<DetalhesTaxaViewModel>(registro);

        return View(detalhesTaxaViewModel);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesTaxaViewModel detalhesTaxaViewModel)
    {
        var registro = servicoTaxa.SelecionarPorId(detalhesTaxaViewModel.Id).Value;
        var resultado = servicoTaxa.Excluir(detalhesTaxaViewModel.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoTaxa.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesTaxaViewModel = mapeador.Map<DetalhesTaxaViewModel>(registro);

        return View(detalhesTaxaViewModel);
    }

    #region
    protected bool ValidarFalha(Result<Taxa> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidarFalhaLista(Result<List<Taxa>> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    #endregion
}