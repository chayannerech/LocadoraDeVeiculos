using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDeCondutor.WebApp.Controllers;
public class CondutorController(CondutorService servicoCondutor, ClienteService servicoCliente, AluguelService servicoAluguel, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoCondutor.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        if (servicoCondutor.SemRegistros() && ViewBag.Mensagem is null)
            ApresentarMensagemSemRegistros();

        var listarCondutorVm = mapeador.Map<IEnumerable<ListarCondutorViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarCondutorVm);
    }


    public IActionResult Inserir()
    {
        if (servicoCliente.SemRegistros())
        {
            ApresentarMensagemSemDependencias("Clientes");
            return RedirectToAction(nameof(Listar));
        }
        return View(CarregarInformacoes(new InserirCondutorViewModel()));
    }
    [HttpPost]
    public IActionResult Inserir(InserirCondutorViewModel inserirRegistroVm)
    {   
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirRegistroVm));
            
        var novoRegistro = mapeador.Map<Condutor>(inserirRegistroVm);

        if (servicoCondutor.ValidarRegistroRepetido(inserirRegistroVm.Check, novoRegistro, out string itemRepetido))
        {
            MostrarMensagemDeRegistroRepetido(itemRepetido);
            return View(CarregarInformacoes(inserirRegistroVm));
        }

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoCondutor.Inserir(novoRegistro, inserirRegistroVm.ClienteId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var resultado = servicoCondutor.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var editarRegistroVm = mapeador.Map<EditarCondutorViewModel>(registro);

        editarRegistroVm.ClienteId = registro.Cliente.Id;

        return View(CarregarInformacoes(editarRegistroVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarCondutorViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(editarRegistroVm));

        var registro = mapeador.Map<Condutor>(editarRegistroVm);

        if (servicoCondutor.ValidarRegistroRepetido(editarRegistroVm.Check, registro, out string itemRepetido))
        {
            MostrarMensagemDeRegistroRepetido(itemRepetido);
            return View(CarregarInformacoes(editarRegistroVm));
        }

        var resultado = servicoCondutor.Editar(registro, editarRegistroVm.ClienteId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoAluguel.AtualizarCondutorDoAluguel(registro);

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoCondutor.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var detalhesRegistroVm = mapeador.Map<DetalhesCondutorViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesCondutorViewModel detalhesRegistroVm)
    {
        var resultado = servicoCondutor.Excluir(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{servicoCondutor.SelecionarPorId(detalhesRegistroVm.Id).Value.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoCondutor.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesCondutorViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    #region Auxiliares
    private InserirCondutorViewModel? CarregarInformacoes(InserirCondutorViewModel inserirCondutorVm)
    {
        var resultadoClientes = servicoCliente.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoClientes.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        inserirCondutorVm.Clientes = resultadoClientes.Value;

        return inserirCondutorVm;
    }
    private EditarCondutorViewModel? CarregarInformacoes(EditarCondutorViewModel editarCondutorVm)
    {
        var resultadoClientes = servicoCliente.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoClientes.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        editarCondutorVm.Clientes = resultadoClientes.Value;

        return editarCondutorVm;
    }
    protected bool ValidarFalha(Result<Condutor> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidarFalhaLista(Result<List<Condutor>> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private void MostrarMensagemDeRegistroRepetido(string itemRepetido)
    {
        if (itemRepetido == "cpf")
            ApresentarMensagemRegistroExistente("Já existe uma pessoa com este CPF");
        if (itemRepetido == "cnh")
            ApresentarMensagemRegistroExistente("Já existe uma pessoa com esta CNH");
    }
    #endregion
}