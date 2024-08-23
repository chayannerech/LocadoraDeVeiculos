using AutoMapper;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class GrupoDeAutomoveisController(GrupoDeAutomoveisService servicoGrupoDeAutomoveis, IMapper mapeador) : WebControllerBase
{
    private readonly IMapper mapeador = mapeador;
    public IActionResult Listar()
    {
        var resultado = servicoGrupoDeAutomoveis.SelecionarTodos(UsuarioId.GetValueOrDefault());

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
    public IActionResult Inserir(InserirGrupoDeAutomoveisViewModel inserirGrupoDeAutomoveisVm)
    {
        if (!ModelState.IsValid)
            return View(inserirGrupoDeAutomoveisVm);

        var novoRegistro = mapeador.Map<GrupoDeAutomoveis>(inserirGrupoDeAutomoveisVm);

        var registrosExistentes = servicoGrupoDeAutomoveis.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        if (registrosExistentes.Exists(r => r.Nome == novoRegistro.Nome))
        {
            ApresentarMensagemRegistroExistente();
            return View(inserirGrupoDeAutomoveisVm); 
        }

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoGrupoDeAutomoveis.Inserir(novoRegistro);

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
        var resultado = servicoGrupoDeAutomoveis.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var editarGrupoDeAutomoveisVm = mapeador.Map<EditarGrupoDeAutomoveisViewModel>(registro);

        return View(editarGrupoDeAutomoveisVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarGrupoDeAutomoveisViewModel editarGrupoDeAutomoveisVm)
    {
        if (!ModelState.IsValid)
            return View(editarGrupoDeAutomoveisVm);

        var registro = mapeador.Map<GrupoDeAutomoveis>(editarGrupoDeAutomoveisVm);

        var registrosExistentes = servicoGrupoDeAutomoveis.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        if (registrosExistentes.Exists(r => r.Nome == registro.Nome))
        {
            ApresentarMensagemRegistroExistente();
            return View(editarGrupoDeAutomoveisVm);
        }

        var resultado = servicoGrupoDeAutomoveis.Editar(registro);

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
        var resultado = servicoGrupoDeAutomoveis.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesGrupoDeAutomoveisViewModel = mapeador.Map<DetalhesGrupoDeAutomoveisViewModel>(registro);

        if (registro.Planos.Count != 0)
            ApresentarMensagemImpossivelExcluir();

        return View(detalhesGrupoDeAutomoveisViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesGrupoDeAutomoveisViewModel detalhesGrupoDeAutomoveisViewModel)
    {
        var nome = servicoGrupoDeAutomoveis.SelecionarPorId(detalhesGrupoDeAutomoveisViewModel.Id).Value.Nome;
        var resultado = servicoGrupoDeAutomoveis.Excluir(detalhesGrupoDeAutomoveisViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var resultado = servicoGrupoDeAutomoveis.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesGrupoDeAutomoveisViewModel = mapeador.Map<DetalhesGrupoDeAutomoveisViewModel>(registro);

        return View(detalhesGrupoDeAutomoveisViewModel);
    }
}