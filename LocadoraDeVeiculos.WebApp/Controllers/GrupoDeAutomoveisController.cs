﻿using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class GrupoDeAutomoveisController(GrupoDeAutomoveisService servicoGrupo, VeiculoService servicoVeiculo, AluguelService servicoAluguel, PlanoDeCobrancaService servicoPlano, FuncionarioService servicoFuncionario, IMapper mapeador) : WebControllerBase(servicoFuncionario)
{
    public IActionResult Listar()
    {
       var resultado = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (!User.Identity!.IsAuthenticated) 
            resultado = servicoGrupo.SelecionarTodos();

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        if (!User.Identity!.IsAuthenticated) 
        {
            ViewBag.Inserir = false;
            if (servicoGrupo.SemRegistros()) ApresentarMensagemSemRegistros();
        }
        else
            if (servicoGrupo.SemRegistros(UsuarioId.GetValueOrDefault()))
                ApresentarMensagemSemRegistros();

        var listarRegistrosVm = mapeador.Map<IEnumerable<ListarGrupoDeAutomoveisViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarRegistrosVm);
    }


    [Authorize(Roles = "Empresa, Funcionario")]
    public IActionResult Inserir() => View();
    [HttpPost]
    public IActionResult Inserir(InserirGrupoDeAutomoveisViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(inserirRegistroVm);

        var novoRegistro = mapeador.Map<GrupoDeAutomoveis>(inserirRegistroVm);

        if (servicoGrupo.ValidarRegistroRepetido(novoRegistro, UsuarioId.GetValueOrDefault()))
        {
            ApresentarMensagemRegistroExistente("Já existe um grupo com este nome");
            return View(inserirRegistroVm);
        }

        novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoGrupo.Inserir(novoRegistro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    [Authorize(Roles = "Empresa, Funcionario")]
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

        if (servicoGrupo.ValidarRegistroRepetido(registro, UsuarioId.GetValueOrDefault()))
        {
            ApresentarMensagemRegistroExistente("Já existe um grupo com este nome");
            return View(editarRegistroVm);
        }

        var resultado = servicoGrupo.Editar(registro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    [Authorize(Roles = "Empresa, Funcionario")]
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
        var registro = servicoGrupo.SelecionarPorId(detalhesRegistroVm.Id).Value;
        var resultado = servicoGrupo.Desativar(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoGrupo.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        ViewBag.Plano = servicoPlano.SelecionarPorGrupoId(id).IsFailed ? null : servicoPlano.SelecionarPorGrupoId(id).Value;

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