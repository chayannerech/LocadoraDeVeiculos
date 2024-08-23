using AutoMapper;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class PlanoDeCobrancaController(PlanoDeCobrancaService servicoPlanoDeCobranca, IMapper mapeador) : WebControllerBase
{
    private readonly IMapper mapeador = mapeador;
    public IActionResult Listar()
    {
        var resultado =
            servicoPlanoDeCobranca.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return RedirectToAction("Index", "Inicio");
        }

        var registros = resultado.Value;

        registros = [new()];

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarPlanoDeCobrancaVm = mapeador.Map<IEnumerable<ListarPlanoDeCobrancaViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarPlanoDeCobrancaVm);
    }

    public IActionResult Inserir() => View();

    [HttpPost]
    public IActionResult Inserir(InserirPlanoDeCobrancaViewModel inserirPlanoDeCobrancaVm)
    {
        if (!ModelState.IsValid)
            return View(inserirPlanoDeCobrancaVm);

        var novoRegistro = mapeador.Map<PlanoDeCobranca>(inserirPlanoDeCobrancaVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoPlanoDeCobranca.Inserir(novoRegistro);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var resultado = servicoPlanoDeCobranca.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var editarPlanoDeCobrancaVm = mapeador.Map<EditarPlanoDeCobrancaViewModel>(registro);

        return View(editarPlanoDeCobrancaVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarPlanoDeCobrancaViewModel editarPlanoDeCobrancaVm)
    {
        if (!ModelState.IsValid)
            return View(editarPlanoDeCobrancaVm);

        var registro = mapeador.Map<PlanoDeCobranca>(editarPlanoDeCobrancaVm);

        var resultado = servicoPlanoDeCobranca.Editar(registro);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var resultado = servicoPlanoDeCobranca.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesPlanoDeCobrancaViewModel = mapeador.Map<DetalhesPlanoDeCobrancaViewModel>(registro);

        return View(detalhesPlanoDeCobrancaViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesPlanoDeCobrancaViewModel detalhesPlanoDeCobrancaViewModel)
    {
        var resultado = servicoPlanoDeCobranca.Excluir(detalhesPlanoDeCobrancaViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{detalhesPlanoDeCobrancaViewModel.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var resultado = servicoPlanoDeCobranca.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesPlanoDeCobrancaViewModel = mapeador.Map<DetalhesPlanoDeCobrancaViewModel>(registro);

        return View(detalhesPlanoDeCobrancaViewModel);
    }
}