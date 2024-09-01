using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class GrupoDeAutomoveisController(GrupoDeAutomoveisService servicoGrupo, VeiculoService servicoVeiculo, AluguelService servicoAluguel, PlanoDeCobrancaService servicoPlano, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        if (servicoGrupo.SemRegistros())
            ApresentarMensagemSemRegistros();

        var listarRegistrosVm = mapeador.Map<IEnumerable<ListarGrupoDeAutomoveisViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarRegistrosVm);
    }


    public IActionResult Inserir() => View();
    [HttpPost]
    public IActionResult Inserir(InserirGrupoDeAutomoveisViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(inserirRegistroVm);

        var novoRegistro = mapeador.Map<GrupoDeAutomoveis>(inserirRegistroVm);

        if (servicoGrupo.ValidarRegistroRepetido(novoRegistro))
        {
            ApresentarMensagemRegistroExistente("Já existe um grupo com este nome");
            return View(inserirRegistroVm);
        }

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoGrupo.Inserir(novoRegistro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoGrupo.SelecionarPorId(id);

        if (ValidarFalha(resultado))
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

        if (servicoGrupo.ValidarRegistroRepetido(registro))
        {
            ApresentarMensagemRegistroExistente("Já existe um grupo com este nome");
            return View(editarRegistroVm);
        }

        var resultado = servicoGrupo.Editar(registro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoAluguel.AtualizarGrupoDoAluguel(registro);

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoGrupo.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoVeiculo.VeiculoRelacionadoAoGrupo(registro) || servicoPlano.PlanoRelacionadoAoGrupo(registro))
        {
            ApresentarMensagemImpossivelExcluir("Existe um veículo ou plano relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var detalhesRegistroVm = mapeador.Map<DetalhesGrupoDeAutomoveisViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesGrupoDeAutomoveisViewModel detalhesRegistroVm)
    {
        var resultado = servicoGrupo.Excluir(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{servicoGrupo.SelecionarPorId(detalhesRegistroVm.Id).Value.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoGrupo.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        ViewBag.Plano = servicoPlano.SelecionarPorGrupoId(id); ;

        var detalhesRegistroVm = mapeador.Map<DetalhesGrupoDeAutomoveisViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    #region
    protected bool ValidarFalha(Result<GrupoDeAutomoveis> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    protected bool ValidarFalhaLista(Result<List<GrupoDeAutomoveis>> resultado)
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