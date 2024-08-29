using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class ClienteController(ClienteService servicoCliente, CondutorService servicoCondutor, IMapper mapeador) : WebControllerBase
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

    public IActionResult Inserir() => View(CarregarInformacoes(new InserirClienteViewModel()));
    [HttpPost]
    public IActionResult Inserir(InserirClienteViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
        {
            if(inserirRegistroVm.PessoaFisica is false)
            {
                inserirRegistroVm.CNH = "";
                inserirRegistroVm.RG = "";
            }
            else
                return View(CarregarInformacoes(inserirRegistroVm));
        }

        var novoRegistro = mapeador.Map<Cliente>(inserirRegistroVm);

        if (ValidacaoDeRegistroRepetido(servicoCliente, novoRegistro, null))
            return View(CarregarInformacoes(inserirRegistroVm));

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

        var editarRegistroVm = mapeador.Map<EditarClienteViewModel>(registro);

        return View(CarregarInformacoes(editarRegistroVm));
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
                return View(CarregarInformacoes(editarRegistroVm));
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

        var detalhesRegistroVm = mapeador.Map<DetalhesClienteViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesClienteViewModel detalhesRegistroVm)
    {
        var nome = servicoCliente.SelecionarPorId(detalhesRegistroVm.Id).Value.Nome;
        var resultado = servicoCliente.Excluir(detalhesRegistroVm.Id);

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

        var detalhesRegistroVm = mapeador.Map<DetalhesClienteViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    #region
    private InserirClienteViewModel? CarregarInformacoes(InserirClienteViewModel inserirRegistroVm)
    {
        inserirRegistroVm.Estados = Enum.GetNames(typeof(EstadosEnum)).Select(t =>
            new SelectListItem(t, t));

        return inserirRegistroVm;
    }
    private EditarClienteViewModel? CarregarInformacoes(EditarClienteViewModel editarRegistroVm)
    {
        editarRegistroVm.Estados = Enum.GetNames(typeof(EstadosEnum)).Select(t =>
            new SelectListItem(t, t));

        return editarRegistroVm;
    }
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
        var cpfsCondutores = servicoCondutor
            .SelecionarTodos(UsuarioId.GetValueOrDefault()).Value
            .Select(r => r.CPF);

        var cnhCondutores = servicoCondutor
            .SelecionarTodos(UsuarioId.GetValueOrDefault()).Value
            .Select(r => r.CNH);

        var cpfsClientes = servicoCliente
            .SelecionarTodos(UsuarioId.GetValueOrDefault()).Value.FindAll(c => c.PessoaFisica)
            .Select(c => c.Documento);

        var cnpjExistente = servicoCliente
            .SelecionarTodos(UsuarioId.GetValueOrDefault()).Value.FindAll(c => !c.PessoaFisica)
            .Select(c => c.Documento);

        var cnhClientes = servicoCliente
            .SelecionarTodos(UsuarioId.GetValueOrDefault()).Value.FindAll(c => c.PessoaFisica)
            .Select(c => c.CNH);

        var rgExistentes = servicoCliente
            .SelecionarTodos(UsuarioId.GetValueOrDefault()).Value.FindAll(c => c.PessoaFisica)
            .Select(c => c.RG);

        IEnumerable<string> cpfsExistentes = cpfsCondutores.Concat(cpfsClientes);
        IEnumerable<string> cnhExistentes = cnhCondutores.Concat(cnhClientes);

        registroAtual = registroAtual is null ? new() : registroAtual;

        if (novoRegistro.PessoaFisica)
        {
            if (cpfsExistentes.Any(c => c == novoRegistro.Documento) && novoRegistro.Documento != registroAtual.Documento)
            {
                ApresentarMensagemRegistroExistente("Já existe um cadastro com esse CPF");
                return true;
            }
            if (cnhExistentes.Any(c => c == novoRegistro.CNH) && novoRegistro.CNH != registroAtual.CNH)
            {
                ApresentarMensagemRegistroExistente("Já existe um cadastro com essa CNH");
                return true;
            }
            if (rgExistentes.Any(c => c == novoRegistro.RG) && novoRegistro.RG != registroAtual.RG)
            {
                ApresentarMensagemRegistroExistente("Já existe um cadastro com esse RG");
                return true;
            }
        }
        else if (cnpjExistente.Any(c => c == novoRegistro.Documento) && novoRegistro.Documento != registroAtual.Documento)
        {
            ApresentarMensagemRegistroExistente("Já existe um cadastro com esse CNPJ");
            return true;
        }

        return false;
    }
    #endregion
}