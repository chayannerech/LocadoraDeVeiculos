using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class GrupoDeAutomoveisController(GrupoDeAutomoveisService servicoGrupo, VeiculoService servicoVeiculo, PlanoDeCobrancaService servicoPlano, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return RedirectToAction("Index", "Inicio");
        }

        var registros = resultado.Value;

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarGrupoDeAutomoveisVm = mapeador.Map<IEnumerable<ListarGrupoDeAutomoveisViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarGrupoDeAutomoveisVm);
    }


    public IActionResult Inserir() => View();
    [HttpPost]
    public IActionResult Inserir(InserirGrupoDeAutomoveisViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(inserirRegistroVm);

        var novoRegistro = mapeador.Map<GrupoDeAutomoveis>(inserirRegistroVm);

        if (ValidacaoDeRegistroRepetido(servicoGrupo, novoRegistro, null))
            return View(inserirRegistroVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoGrupo.Inserir(novoRegistro);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoGrupo.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var editarGrupoDeAutomoveisVm = mapeador.Map<EditarGrupoDeAutomoveisViewModel>(registro);

        return View(editarGrupoDeAutomoveisVm);
    }    
    [HttpPost]
    public IActionResult Editar(EditarGrupoDeAutomoveisViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(editarRegistroVm);

        var registro = mapeador.Map<GrupoDeAutomoveis>(editarRegistroVm);
        var registroAtual = servicoGrupo.SelecionarPorId(editarRegistroVm.Id).Value;

        if (ValidacaoDeRegistroRepetido(servicoGrupo, registro, registroAtual))
            return View(editarRegistroVm);

        var resultado = servicoGrupo.Editar(registro);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoGrupo.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesGrupoDeAutomoveisViewModel = mapeador.Map<DetalhesGrupoDeAutomoveisViewModel>(registro);

        if (ValidarPossibilidadeDeExclusao(servicoVeiculo, registro))
            return RedirectToAction(nameof(Listar));

        if (ValidarPossibilidadeDeExclusao(servicoPlano, registro))
            return RedirectToAction(nameof(Listar));

        return View(detalhesGrupoDeAutomoveisViewModel);
    }


    [HttpPost]
    public IActionResult Excluir(DetalhesGrupoDeAutomoveisViewModel detalhesGrupoDeAutomoveisViewModel)
    {
        var nome = servicoGrupo.SelecionarPorId(detalhesGrupoDeAutomoveisViewModel.Id).Value.Nome;
        var resultado = servicoGrupo.Excluir(detalhesGrupoDeAutomoveisViewModel.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoGrupo.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var planoAssociado = servicoPlano.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value.Find(p => p.GrupoDeAutomoveis.Id == registro.Id);

        ViewBag.Plano = planoAssociado;

        var detalhesGrupoDeAutomoveisViewModel = mapeador.Map<DetalhesGrupoDeAutomoveisViewModel>(registro);

        return View(detalhesGrupoDeAutomoveisViewModel);
    }

    #region
    protected bool ValidacaoDeFalha(Result<GrupoDeAutomoveis> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidacaoDeRegistroRepetido(GrupoDeAutomoveisService servicoGrupoDeAutomoveis, GrupoDeAutomoveis novoRegistro, GrupoDeAutomoveis registroAtual)
    {
        var registrosExistentes = servicoGrupoDeAutomoveis.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        registroAtual = registroAtual is null ? new() { Nome = ""} : registroAtual;

        if (registrosExistentes.Exists(r =>
            r.Nome.Validation() == novoRegistro.Nome.Validation() &&
            r.Nome.Validation() != registroAtual.Nome.Validation()))
        {
            ApresentarMensagemRegistroExistente("Já existe um grupo com este nome");
            return true;
        }
        return false;
    }
    private bool ValidarPossibilidadeDeExclusao(VeiculoService servicoVeiculo, GrupoDeAutomoveis registro)
    {
        foreach (var veiculo in servicoVeiculo.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value)
            if (veiculo.GrupoDeAutomoveis.Id == registro.Id)
            {
                ApresentarMensagemImpossivelExcluir();
                return true;
            }

        return false;
    }
    private bool ValidarPossibilidadeDeExclusao(PlanoDeCobrancaService servicoPlano, GrupoDeAutomoveis registro)
    {
        foreach (var plano in servicoPlano.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value)
            if (plano.GrupoDeAutomoveis.Id == registro.Id)
            {
                ApresentarMensagemImpossivelExcluir();
                return true;
            }

        return false;
    }
    #endregion
}