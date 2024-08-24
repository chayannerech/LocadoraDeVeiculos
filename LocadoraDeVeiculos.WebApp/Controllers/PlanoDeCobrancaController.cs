using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDePlanoDeCobranca.WebApp.Controllers;
public class PlanoDeCobrancaController(PlanoDeCobrancaService servicoPlanos, GrupoDeAutomoveisService servicoGrupos, IMapper mapeador) : WebControllerBase
{
    private readonly IMapper mapeador = mapeador;
    public IActionResult Listar()
    {
        var resultado =
            servicoPlanos.SelecionarTodos(UsuarioId.GetValueOrDefault());

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
    public IActionResult Inserir(InserirPlanoDeCobrancaViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirRegistroVm));

        var novoRegistro = mapeador.Map<PlanoDeCobranca>(inserirRegistroVm);

        var registrosExistentes = servicoPlanos.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        if (registrosExistentes.Exists(r => r.GrupoDeAutomoveis.Id == inserirRegistroVm.GrupoId))
        {
            ApresentarMensagemRegistroExistente();
            return View(CarregarInformacoes(inserirRegistroVm));
        }

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoPlanos.Inserir(novoRegistro, inserirRegistroVm.GrupoId);

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
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var editarPlanoVm = mapeador.Map<EditarPlanoDeCobrancaViewModel>(registro);

        editarPlanoVm.GrupoId = registro.GrupoDeAutomoveis.Id;

        return View(CarregarInformacoes(editarPlanoVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarPlanoDeCobrancaViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(editarRegistroVm);

        var registro = mapeador.Map<PlanoDeCobranca>(editarRegistroVm);

        var registroAtual = servicoPlanos.SelecionarPorId(editarRegistroVm.Id).Value;
        var registrosExistentes = servicoPlanos.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        if (registrosExistentes.Exists(r => r.GrupoDeAutomoveis.Id == editarRegistroVm.GrupoId && r.GrupoDeAutomoveis.Id != registroAtual.GrupoDeAutomoveis.Id))
        {
            ApresentarMensagemRegistroExistente();
            return View(CarregarInformacoes(editarRegistroVm));
        }

        var resultado = servicoPlanos.Editar(registro, editarRegistroVm.GrupoId);

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
        var resultado = servicoPlanos.SelecionarPorId(id);

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
        var resultado = servicoPlanos.Excluir(detalhesPlanoDeCobrancaViewModel.Id);

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
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesPlanoDeCobrancaViewModel = mapeador.Map<DetalhesPlanoDeCobrancaViewModel>(registro);

        return View(detalhesPlanoDeCobrancaViewModel);
    }

    #region Auxiliares
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
    #endregion
}