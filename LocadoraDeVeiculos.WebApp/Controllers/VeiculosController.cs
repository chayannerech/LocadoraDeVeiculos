using AutoMapper;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class VeiculosController(VeiculosService servicoVeiculos, IMapper mapeador) : WebControllerBase
{
    private readonly IMapper mapeador = mapeador;
    public IActionResult Listar()
    {
        var resultado =
            servicoVeiculos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction("Index", "Inicio");
        }

        var registros = resultado.Value;

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarVeiculosVm = mapeador.Map<IEnumerable<ListarVeiculosViewModel>>(registros);

        return View(listarVeiculosVm);
    }

    public IActionResult Inserir() => View();

    [HttpPost]
    public IActionResult Inserir(InserirVeiculosViewModel inserirVeiculosVm)
    {
        if (!ModelState.IsValid)
            return View(inserirVeiculosVm);

        var novoRegistro = mapeador.Map<Veiculos>(inserirVeiculosVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoVeiculos.Inserir(novoRegistro);

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
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var editarVeiculosVm = mapeador.Map<EditarVeiculosViewModel>(registro);

        return View(editarVeiculosVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarVeiculosViewModel editarVeiculosVm)
    {
        if (!ModelState.IsValid)
            return View(editarVeiculosVm);

        var registro = mapeador.Map<Veiculos>(editarVeiculosVm);

        var resultado = servicoVeiculos.Editar(registro);

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
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        //registro.Planos = [new("1"), new("2")];
        registro.Planos = [];

        var detalhesVeiculosViewModel = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        if (registro.Planos.Count != 0)
            ApresentarMensagemImpossivelExcluir();

        return View(detalhesVeiculosViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesVeiculosViewModel detalhesVeiculosViewModel)
    {
        var resultado = servicoVeiculos.Excluir(detalhesVeiculosViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{detalhesVeiculosViewModel.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        registro.Planos = [new("1", registro), new("2", registro)];

        var detalhesVeiculosViewModel = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        return View(detalhesVeiculosViewModel);
    }
}