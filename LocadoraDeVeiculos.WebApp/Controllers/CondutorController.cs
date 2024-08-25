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
public class CondutorController(CondutorService servicoPlanos, ClienteService servicoClientes, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoPlanos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return RedirectToAction("Index", "Inicio");
        }

        var registros = resultado.Value;

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarCondutorVm = mapeador.Map<IEnumerable<ListarCondutorViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarCondutorVm);
    }


    public IActionResult Inserir() => View(CarregarInformacoes(new InserirCondutorViewModel()));
    [HttpPost]
    public IActionResult Inserir(InserirCondutorViewModel inserirRegistroVm)
    {   
        if (!ModelState.IsValid)
        {
            if (inserirRegistroVm.ClienteId != 0)
            {
                var clienteSelecionado = servicoClientes.SelecionarPorId(inserirRegistroVm.ClienteId).Value;
                inserirRegistroVm.Nome = clienteSelecionado.Nome;
                inserirRegistroVm.Email = clienteSelecionado.Email;
                inserirRegistroVm.Telefone = clienteSelecionado.Telefone;
                inserirRegistroVm.CPF = clienteSelecionado.Documento;
                inserirRegistroVm.CNH = clienteSelecionado.CNH;
            }
            else
                return View(CarregarInformacoes(inserirRegistroVm));

            if (inserirRegistroVm.ValidadeCNH < DateTime.Now)
                return View(CarregarInformacoes(inserirRegistroVm));
        }
            
        var novoRegistro = mapeador.Map<Condutor>(inserirRegistroVm);

        if (ValidacaoDeRegistroRepetido(servicoPlanos, inserirRegistroVm, null))
            return View(CarregarInformacoes(inserirRegistroVm));

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoPlanos.Inserir(novoRegistro, inserirRegistroVm.ClienteId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var editarPlanoVm = mapeador.Map<EditarCondutorViewModel>(registro);

        editarPlanoVm.ClienteId = registro.Cliente.Id;

        return View(CarregarInformacoes(editarPlanoVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarCondutorViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(editarRegistroVm);

        var registro = mapeador.Map<Condutor>(editarRegistroVm);
        var registroAtual = servicoPlanos.SelecionarPorId(editarRegistroVm.Id).Value;

        if (ValidacaoDeRegistroRepetido(servicoPlanos, editarRegistroVm, registroAtual))
            return View(editarRegistroVm);

        var resultado = servicoPlanos.Editar(registro, editarRegistroVm.ClienteId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesCondutorViewModel = mapeador.Map<DetalhesCondutorViewModel>(registro);

        return View(detalhesCondutorViewModel);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesCondutorViewModel detalhesCondutorViewModel)
    {
        var resultado = servicoPlanos.Excluir(detalhesCondutorViewModel.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{detalhesCondutorViewModel.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoPlanos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesCondutorViewModel = mapeador.Map<DetalhesCondutorViewModel>(registro);

        return View(detalhesCondutorViewModel);
    }

    #region Auxiliares
    private InserirCondutorViewModel? CarregarInformacoes(InserirCondutorViewModel inserirCondutorVm)
    {
        var resultadoClientes = servicoClientes.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoClientes.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        inserirCondutorVm.Clientes = resultadoClientes.Value.FindAll(c => c.PessoaFisica);

        return inserirCondutorVm;
    }
    private EditarCondutorViewModel? CarregarInformacoes(EditarCondutorViewModel editarCondutorVm)
    {
        var resultadoClientes = servicoClientes.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoClientes.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        editarCondutorVm.Clientes = resultadoClientes.Value;

        return editarCondutorVm;
    }
    protected bool ValidacaoDeFalha(Result<Condutor> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidacaoDeRegistroRepetido(CondutorService servicoCondutor, InserirCondutorViewModel novoRegistro, Condutor registroAtual)
    {
        var registrosExistentes = servicoCondutor.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        registroAtual = registroAtual is null ? new() : registroAtual;

        if (registrosExistentes.Exists(r => (
            r.CPF == novoRegistro.CPF &&
            r.CPF != registroAtual.CPF) || (
            r.CNH == novoRegistro.CNH &&
            r.CNH != registroAtual.CNH)))
        {
            ApresentarMensagemRegistroExistente();
            return true;
        }
        return false;
    }
    #endregion
}