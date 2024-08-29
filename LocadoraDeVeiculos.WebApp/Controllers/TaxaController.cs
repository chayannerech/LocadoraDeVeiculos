using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class TaxaController(TaxaService servicoTaxa, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoTaxa.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return RedirectToAction("Index", "Inicio");
        }

        var registros = resultado.Value;

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarTaxaVm = mapeador.Map<IEnumerable<ListarTaxaViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarTaxaVm);
    }


    public IActionResult Inserir() => View();
    [HttpPost]
    public IActionResult Inserir(InserirTaxaViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(inserirRegistroVm);

        var novoRegistro = mapeador.Map<Taxa>(inserirRegistroVm);

        if (ValidacaoDeRegistroRepetido(servicoTaxa, novoRegistro, null))
            return View(inserirRegistroVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoTaxa.Inserir(novoRegistro);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoTaxa.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var editarTaxaVm = mapeador.Map<EditarTaxaViewModel>(registro);

        return View(editarTaxaVm);
    }    
    [HttpPost]
    public IActionResult Editar(EditarTaxaViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(editarRegistroVm);

        var registro = mapeador.Map<Taxa>(editarRegistroVm);
        var registroAtual = servicoTaxa.SelecionarPorId(editarRegistroVm.Id).Value;

        if (ValidacaoDeRegistroRepetido(servicoTaxa, registro, registroAtual))
            return View(editarRegistroVm);

        var resultado = servicoTaxa.Editar(registro);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoTaxa.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesTaxaViewModel = mapeador.Map<DetalhesTaxaViewModel>(registro);

        return View(detalhesTaxaViewModel);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesTaxaViewModel detalhesTaxaViewModel)
    {
        var nome = servicoTaxa.SelecionarPorId(detalhesTaxaViewModel.Id).Value.Nome;
        var resultado = servicoTaxa.Excluir(detalhesTaxaViewModel.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoTaxa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesTaxaViewModel = mapeador.Map<DetalhesTaxaViewModel>(registro);

        return View(detalhesTaxaViewModel);
    }

    #region
    protected bool ValidacaoDeFalha(Result<Taxa> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidacaoDeRegistroRepetido(TaxaService servicoTaxa, Taxa novoRegistro, Taxa registroAtual)
    {
        var registrosExistentes = servicoTaxa.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        registroAtual = registroAtual is null ? new() { Nome = "" } : registroAtual;

        if (registrosExistentes.Exists(r =>
            r.Nome.Validation() == novoRegistro.Nome.Validation() &&
            r.Nome.Validation() != registroAtual.Nome.Validation()))
        {
            ApresentarMensagemRegistroExistente("Já existe um cadastro com esse nome");
            return true;
        }
        return false;
    }
    #endregion
}