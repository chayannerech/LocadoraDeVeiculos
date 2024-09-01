using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class ConfiguracaoController(ConfiguracaoService servicoConfiguracao, IMapper mapeador) : WebControllerBase
{
    public IActionResult Inserir() => View();
    [HttpPost]
    public IActionResult Inserir(InserirConfiguracaoViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(inserirRegistroVm);

        var novoRegistro = mapeador.Map<Configuracao>(inserirRegistroVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoConfiguracao.Inserir(novoRegistro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Detalhes));

        ApresentarMensagemSucesso($"A configuração dos preços de combustível foi inserida com sucesso!");

        return RedirectToAction(nameof(Detalhes));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoConfiguracao.Selecionar();

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Detalhes));

        return View(mapeador.Map<EditarConfiguracaoViewModel>(resultado));
    }    
    [HttpPost]
    public IActionResult Editar(EditarConfiguracaoViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(editarRegistroVm);

        var registro = mapeador.Map<Configuracao>(editarRegistroVm);

        var resultado = servicoConfiguracao.Editar(registro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Detalhes));

        ApresentarMensagemSucesso($"As configurações foram atualizadas com sucesso!");

        return RedirectToAction(nameof(Detalhes));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoConfiguracao.Selecionar();

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Detalhes));

        return View(mapeador.Map<DetalhesConfiguracaoViewModel>(resultado));
    }


    protected bool ValidarFalha(Result<Configuracao> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
}