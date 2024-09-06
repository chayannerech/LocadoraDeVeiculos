using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDeVeiculos.WebApp.Controllers;

[Authorize(Roles = "Empresa, Funcionario")]
public class ClienteController(ClienteService servicoCliente, CondutorService servicoCondutor, AluguelService servicoAluguel, FuncionarioService servicoFuncionario, IMapper mapeador) : WebControllerBase(servicoFuncionario)
{
    public IActionResult Listar()
    {
        var resultado = servicoCliente.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        if (servicoCliente.SemRegistros(UsuarioId.GetValueOrDefault()))
            ApresentarMensagemSemRegistros();

        var listarClienteVm = mapeador.Map<IEnumerable<ListarClienteViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarClienteVm);
    }

    public IActionResult Inserir() => View(CarregarInformacoes(new InserirClienteViewModel()));
    [HttpPost]
    public IActionResult Inserir(InserirClienteViewModel inserirRegistroVm)
    {
        AjustarDocumentosPessoaJuridica(inserirRegistroVm);

        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirRegistroVm));

        var novoRegistro = mapeador.Map<Cliente>(inserirRegistroVm);

        if (servicoCliente.ValidarRegistroRepetido(novoRegistro, out string itemRepetido, UsuarioId.GetValueOrDefault()))
        {
            EnviarMensagemDeRegistroRepetido(itemRepetido);
            return View(CarregarInformacoes(inserirRegistroVm));
        }

        novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoCliente.Inserir(novoRegistro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoCliente.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var editarRegistroVm = mapeador.Map<EditarClienteViewModel>(registro);

        return View(CarregarInformacoes(editarRegistroVm));
    }    
    [HttpPost]
    public IActionResult Editar(EditarClienteViewModel editarRegistroVm)
    {
        AjustarDocumentosPessoaJuridica(editarRegistroVm);

        if (!ModelState.IsValid)
            return View(CarregarInformacoes(editarRegistroVm));

        var registro = mapeador.Map<Cliente>(editarRegistroVm);

        if (servicoCliente.ValidarRegistroRepetido(registro, out string itemRepetido, UsuarioId.GetValueOrDefault()))
        {
            EnviarMensagemDeRegistroRepetido(itemRepetido);
            return View(CarregarInformacoes(editarRegistroVm));
        }

        var resultado = servicoCliente.Editar(registro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var resultado = servicoCliente.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelExcluir("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        if (servicoCondutor.CondutorRelacionado(registro))
        {
            ApresentarMensagemImpossivelExcluir("Existe um condutor relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var detalhesRegistroVm = mapeador.Map<DetalhesClienteViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesClienteViewModel detalhesRegistroVm)
    {
        var registro = servicoCliente.SelecionarPorId(detalhesRegistroVm.Id).Value;
        var resultado = servicoCliente.Desativar(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro.Nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoCliente.SelecionarPorId(id);

        if (ValidarFalha(resultado))
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
    protected bool ValidarFalha(Result<Cliente> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    protected bool ValidarFalhaLista(Result<List<Cliente>> resultado)
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
    private void AjustarDocumentosPessoaJuridica(InserirClienteViewModel inserir)
    {
        if (inserir.PessoaFisica is false)
        {
            inserir.CNH = "";
            inserir.RG = "";

            ModelState.Remove(nameof(inserir.CNH));
            ModelState.Remove(nameof(inserir.RG));
        }
    }
    private void AjustarDocumentosPessoaJuridica(EditarClienteViewModel editarRegistroVm)
    {
        if (editarRegistroVm.PessoaFisica is false)
        {
            editarRegistroVm.CNH = "";
            editarRegistroVm.RG = "";

            ModelState.Remove(nameof(editarRegistroVm.CNH));
            ModelState.Remove(nameof(editarRegistroVm.RG));
        }
    }
    #endregion
}