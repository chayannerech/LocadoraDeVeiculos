using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDePlanoDeCobranca.WebApp.Controllers;
public class PlanoDeCobrancaController(PlanoDeCobrancaService servicoPlanos, GrupoDeAutomoveisService servicoGrupos, IMapper mapeador) : WebControllerBase
{
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

        if (ValidacaoDeRegistroRepetido(servicoPlanos, inserirRegistroVm, null))
            return View(inserirRegistroVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoPlanos.Inserir(novoRegistro, inserirRegistroVm.GrupoId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoGrupos.AdicionarValores(novoRegistro);

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

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

        if (ValidacaoDeRegistroRepetido(servicoPlanos, editarRegistroVm, registroAtual))
            return View(editarRegistroVm);

        var resultado = servicoPlanos.Editar(registro, editarRegistroVm.GrupoId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoGrupos.AdicionarValores(servicoPlanos.SelecionarPorId(registro.Id).Value);

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesPlanoDeCobrancaViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesPlanoDeCobrancaViewModel detalhesRegistroVm)
    {
        var resultado = servicoPlanos.Excluir(detalhesRegistroVm.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"Plano para o grupo: {detalhesRegistroVm.GrupoNome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesPlanoDeCobrancaViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    #region Auxiliares
    private InserirPlanoDeCobrancaViewModel? CarregarInformacoes(InserirPlanoDeCobrancaViewModel inserirRegistroVm)
    {
        var resultadoGrupos = servicoGrupos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        var grupos = resultadoGrupos.Value;

        inserirRegistroVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        inserirRegistroVm.Categorias = new(Enum.GetNames(typeof(CategoriaDePlanoEnum)));

        return inserirRegistroVm;
    }
    private EditarPlanoDeCobrancaViewModel? CarregarInformacoes(EditarPlanoDeCobrancaViewModel editarRegistroVm)
    {
        var resultadoGrupos = servicoGrupos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        var grupos = resultadoGrupos.Value;

        editarRegistroVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        editarRegistroVm.Categorias = new(Enum.GetNames(typeof(CategoriaDePlanoEnum)));

        return editarRegistroVm;
    }
    protected bool ValidacaoDeFalha(Result<PlanoDeCobranca> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidacaoDeRegistroRepetido(PlanoDeCobrancaService servicoPlanoDeCobranca, InserirPlanoDeCobrancaViewModel novoRegistro, PlanoDeCobranca registroAtual)
    {
        var registrosExistentes = servicoPlanoDeCobranca.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        registroAtual = registroAtual is null ? new() : registroAtual;

        if (registrosExistentes.Exists(r =>
            r.GrupoDeAutomoveis.Id == novoRegistro.GrupoId &&
            r.GrupoDeAutomoveis.Id != registroAtual.GrupoDeAutomoveis.Id))
        {
            ApresentarMensagemRegistroExistente();
            return true;
        }
        return false;
    }
    #endregion
}