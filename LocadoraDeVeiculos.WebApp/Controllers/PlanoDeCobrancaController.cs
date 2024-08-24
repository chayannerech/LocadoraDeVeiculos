using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDePlanoDeCobranca.WebApp.Controllers;
public class PlanoDeCobrancaController(PlanoDeCobrancaService servicoPlanoDeCobranca, GrupoDeAutomoveisService servicoGrupos, IMapper mapeador) : WebControllerBase
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

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarPlanoDeCobrancaVm = mapeador.Map<IEnumerable<ListarPlanoDeCobrancaViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarPlanoDeCobrancaVm);
    }

    public IActionResult Inserir() => View(CarregarInformacoes(new InserirPlanoDeCobrancaViewModel()));

    [HttpPost]
    public IActionResult Inserir(InserirPlanoDeCobrancaViewModel inserirPlanoDeCobrancaVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirPlanoDeCobrancaVm));

        var novoRegistro = mapeador.Map<PlanoDeCobranca>(inserirPlanoDeCobrancaVm);

        var registrosExistentes = servicoPlanoDeCobranca.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        if (registrosExistentes.Exists(r => r.GrupoDeAutomoveis.Id == inserirPlanoDeCobrancaVm.GrupoId && r.Categoria == novoRegistro.Categoria))
        {
            ApresentarMensagemRegistroExistente();
            return View(CarregarInformacoes(inserirPlanoDeCobrancaVm));
        }

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoPlanoDeCobranca.Inserir(novoRegistro, inserirPlanoDeCobrancaVm.GrupoId);

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

    private InserirPlanoDeCobrancaViewModel? CarregarInformacoes(InserirPlanoDeCobrancaViewModel inserirPlanoDeCobrancaVm)
    {
        var resultadoGrupos = servicoGrupos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));

            return null;
        }

        var grupos = resultadoGrupos.Value;

        inserirPlanoDeCobrancaVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        return inserirPlanoDeCobrancaVm;
    }
    private EditarPlanoDeCobrancaViewModel? CarregarInformacoes(EditarPlanoDeCobrancaViewModel editarPlanoDeCobrancaVm)
    {
        var resultadoGrupos = servicoGrupos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));

            return null;
        }

        var grupos = resultadoGrupos.Value;

        editarPlanoDeCobrancaVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        return editarPlanoDeCobrancaVm;
    }
}