using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;

[Authorize(Roles = "Empresa")]
public class FuncionarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, RoleManager<Perfil> roleManager, FuncionarioService servicoFuncionario, IMapper mapeador) : WebControllerBase(servicoFuncionario)
{
    public IActionResult Listar()
    {
        var resultado = servicoFuncionario.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        if (servicoFuncionario.SemRegistros() && ViewBag.Mensagem is null)
            ApresentarMensagemSemRegistros();

        var listarFuncionarioVm = mapeador.Map<IEnumerable<ListarFuncionarioViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarFuncionarioVm);
    }

    public IActionResult Inserir() => View();
    [HttpPost]
    public async Task<IActionResult> Inserir(InserirFuncionarioViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
             return View(inserirRegistroVm);

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

        var usuario = new Usuario()
        {
            UserName = inserirRegistroVm.Login,
            Email = inserirRegistroVm.Email
        };

        var resultadoCriacaoUsuario = await userManager.CreateAsync(usuario, inserirRegistroVm.Senha!);
        var resultadoCriacaoTipoUsuario = await roleManager.FindByNameAsync("Funcionario");

        if (resultadoCriacaoTipoUsuario is null)
        {
            var cargo = new Perfil()
            {
                Name = "Funcionario",
                NormalizedName = "FUNCIONARIO",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            await roleManager.CreateAsync(cargo);
        }

        await userManager.AddToRoleAsync(usuario, "Funcionario");

        if (resultadoCriacaoUsuario.Errors.Any())
            ApresentarMensagemFalha("Não foi possível cadastrar este funcionário");
        else
            ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var usuario = await userManager.FindByNameAsync(registro.Login);

        if (ValidarUsuario(usuario, registro, "identificado"))
            return RedirectToAction(nameof(Listar));

        var editarRegistroVm = mapeador.Map<EditarFuncionarioViewModel>(registro);

        editarRegistroVm.Email = usuario!.Email;

        return View(editarRegistroVm);
    }    
    [HttpPost]
    public async Task<IActionResult> Editar(EditarFuncionarioViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
             return View(editarRegistroVm);

        var registro = mapeador.Map<Funcionario>(editarRegistroVm);
        var loginAtual = servicoFuncionario.SelecionarPorId(editarRegistroVm.Id).Value.Login;

        var resultado = servicoFuncionario.Editar(registro);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var usuario = await userManager.FindByNameAsync(loginAtual);

        if (ValidarUsuario(usuario, registro, "editado"))
            return RedirectToAction(nameof(Listar));

        usuario!.UserName = editarRegistroVm.Login;
        usuario!.Email = editarRegistroVm.Email;

        var result = await userManager.UpdateAsync(usuario);

        if (result.Succeeded)
            ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");
        else
            ApresentarMensagemFalha("O registro do funcionário foi editado, mas o login não pôde ser alterado");

        return RedirectToAction(nameof(Listar));
    }


    public async Task<IActionResult> Excluir(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var usuario = await userManager.FindByNameAsync(registro.Login);

        if (ValidarUsuario(usuario, registro, "identificado"))
            return RedirectToAction(nameof(Listar));

        var detalhesRegistroVm = mapeador.Map<DetalhesFuncionarioViewModel>(registro);
        detalhesRegistroVm.Email = usuario!.Email;

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public async Task<IActionResult> Excluir(DetalhesFuncionarioViewModel detalhesRegistroVm)
    {
        var registro = servicoFuncionario.SelecionarPorId(detalhesRegistroVm.Id).Value;
        var usuario = await userManager.FindByNameAsync(registro.Login);

        var resultado = servicoFuncionario.Excluir(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        if (ValidarUsuario(usuario, registro, "excluído"))
            return RedirectToAction(nameof(Listar));

        var result = await userManager.DeleteAsync(usuario!);

        if (result.Succeeded)
            ApresentarMensagemSucesso($"O registro \"{registro.Nome}\" e o login associado foram excluídos com sucesso!");
        else
            ApresentarMensagemFalha("O registro do funcionário foi excluído, mas o login não pôde ser removido");

        return RedirectToAction(nameof(Listar));
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var usuario = await userManager.FindByNameAsync(registro.Login);

        if (ValidarUsuario(usuario, registro, "identificado"))
            return RedirectToAction(nameof(Listar));

        var detalhesRegistroVm = mapeador.Map<DetalhesFuncionarioViewModel>(registro);
        detalhesRegistroVm.Email = usuario!.Email;

        return View(detalhesRegistroVm);
    }

    #region
    protected bool ValidarFalha(Result<Funcionario> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    protected bool ValidarUsuario(Usuario? usuario, Funcionario funcionario, string acao)
    {
        if (usuario is null)
        {
            ApresentarMensagemSucesso($"O registro \"{funcionario}\" foi {acao} com sucesso, mas o login não foi encontrado");
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