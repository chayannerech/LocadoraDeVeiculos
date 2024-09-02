using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDeVeiculos.WebApp.Controllers;

[Authorize(Roles = "Empresa")]
public class FuncionarioController(FuncionarioService servicoFuncionario, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoFuncionario.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        if (servicoFuncionario.SemRegistros())
            ApresentarMensagemSemRegistros();

        var listarFuncionarioVm = mapeador.Map<IEnumerable<ListarFuncionarioViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarFuncionarioVm);
    }

    public IActionResult Inserir() => View(CarregarInformacoes(new InserirFuncionarioViewModel()));
    [HttpPost]
    public IActionResult Inserir(InserirFuncionarioViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
             return View(CarregarInformacoes(inserirRegistroVm));

        var novoRegistro = mapeador.Map<Funcionario>(inserirRegistroVm);

        if (servicoFuncionario.ValidarRegistroRepetido(novoRegistro))
        {
            ApresentarMensagemRegistroExistente("Já existe um cadastro com este login");
            return View(inserirRegistroVm);
        }

        novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoFuncionario.Inserir(novoRegistro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var editarRegistroVm = mapeador.Map<EditarFuncionarioViewModel>(registro);

        return View(CarregarInformacoes(editarRegistroVm));
    }    
    [HttpPost]
    public IActionResult Editar(EditarFuncionarioViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
             return View(CarregarInformacoes(editarRegistroVm));

        var registro = mapeador.Map<Funcionario>(editarRegistroVm);

        var resultado = servicoFuncionario.Editar(registro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesFuncionarioViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesFuncionarioViewModel detalhesRegistroVm)
    {
        var resultado = servicoFuncionario.Excluir(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{servicoFuncionario.SelecionarPorId(detalhesRegistroVm.Id).Value.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesFuncionarioViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    #region
    private InserirFuncionarioViewModel? CarregarInformacoes(InserirFuncionarioViewModel inserirRegistroVm)
    {
        return inserirRegistroVm;
    }
    private EditarFuncionarioViewModel? CarregarInformacoes(EditarFuncionarioViewModel editarRegistroVm)
    {
        return editarRegistroVm;
    }
    protected bool ValidarFalha(Result<Funcionario> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    protected bool ValidarFalhaLista(Result<List<Funcionario>> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private void EnviarMensagemDeRegistroRepetido(string itemRepetido)
    {
        if (itemRepetido == "cpf")
            ApresentarMensagemRegistroExistente("Já existe um cadastro com esse CPF");
        if (itemRepetido == "cnpj")
            ApresentarMensagemRegistroExistente("Já existe um cadastro com esse CNPJ");
        if (itemRepetido == "rg")
            ApresentarMensagemRegistroExistente("Já existe um cadastro com esse RG");
        if (itemRepetido == "cnh")
            ApresentarMensagemRegistroExistente("Já existe um cadastro com essa CNH");
    }
    #endregion
}