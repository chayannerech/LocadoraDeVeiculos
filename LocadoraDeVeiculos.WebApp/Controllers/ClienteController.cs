using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class ClienteController(ClienteService servicoCliente, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoCliente.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return RedirectToAction("Index", "Inicio");
        }

        var registros = resultado.Value;

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarClienteVm = mapeador.Map<IEnumerable<ListarClienteViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarClienteVm);
    }

    public IActionResult Inserir() => View();
    [HttpPost]
    public IActionResult Inserir(InserirClienteViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
        {
            if(inserirRegistroVm.Documento is not null)
            {
                inserirRegistroVm.CNH = "";
                inserirRegistroVm.RG = "";
            }
            else
                return View(inserirRegistroVm);
        }

        var novoRegistro = mapeador.Map<Cliente>(inserirRegistroVm);

        if (ValidacaoDeRegistroRepetido(servicoCliente, novoRegistro, null))
            return View(inserirRegistroVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoCliente.Inserir(novoRegistro);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var resultado = servicoCliente.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var editarClienteVm = mapeador.Map<EditarClienteViewModel>(registro);

        return View(editarClienteVm);
    }    
    [HttpPost]
    public IActionResult Editar(EditarClienteViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
        {
            if (editarRegistroVm.Documento is not null)
            {
                editarRegistroVm.CNH = "";
                editarRegistroVm.RG = "";
            }
            else
                return View(editarRegistroVm);
        }

        var registro = mapeador.Map<Cliente>(editarRegistroVm);
        var registroAtual = servicoCliente.SelecionarPorId(editarRegistroVm.Id).Value;

        if (ValidacaoDeRegistroRepetido(servicoCliente, registro, registroAtual))
            return View(editarRegistroVm);

        var resultado = servicoCliente.Editar(registro);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoCliente.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesClienteViewModel = mapeador.Map<DetalhesClienteViewModel>(registro);

        return View(detalhesClienteViewModel);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesClienteViewModel detalhesClienteViewModel)
    {
        var nome = servicoCliente.SelecionarPorId(detalhesClienteViewModel.Id).Value.Nome;
        var resultado = servicoCliente.Excluir(detalhesClienteViewModel.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoCliente.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesClienteViewModel = mapeador.Map<DetalhesClienteViewModel>(registro);

        return View(detalhesClienteViewModel);
    }

    #region
    protected bool ValidacaoDeFalha(Result<Cliente> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidacaoDeRegistroRepetido(ClienteService servicoCliente, Cliente novoRegistro, Cliente registroAtual)
    {
        var registrosExistentes = servicoCliente.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        registroAtual = registroAtual is null ? new() : registroAtual;

        if (registrosExistentes.Exists(r => 
            r.Documento == novoRegistro.Documento &&
            r.Documento != registroAtual.Documento))
        {
            ApresentarMensagemRegistroExistente();
            return true;
        }
        return false;
    }
    #endregion
}