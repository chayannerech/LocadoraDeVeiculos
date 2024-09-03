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
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDeVeiculos.WebApp.Controllers;

[Authorize(Roles = "Empresa")]
public class FuncionarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, RoleManager<Perfil> roleManager, FuncionarioService servicoFuncionario, IMapper mapeador) : WebControllerBase
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

    public IActionResult Editar(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var editarRegistroVm = mapeador.Map<EditarFuncionarioViewModel>(registro);

        return View(editarRegistroVm);
    }    
    [HttpPost]
    public IActionResult Editar(EditarFuncionarioViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
             return View(editarRegistroVm);

        var registroAtual = servicoFuncionario.SelecionarPorId(editarRegistroVm.Id).Value;
        var registro = mapeador.Map<Funcionario>(editarRegistroVm);

        registro.Email = registroAtual.Email;
        registro.Login = registroAtual.Login;
        registro.Senha = registroAtual.Senha;

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
    public async Task<IActionResult> Excluir(DetalhesFuncionarioViewModel detalhesRegistroVm)
    {
        var registro = servicoFuncionario.SelecionarPorId(detalhesRegistroVm.Id).Value;
        var resultado = servicoFuncionario.Excluir(detalhesRegistroVm.Id);
        var usuario = await userManager.GetUserAsync(User);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        if (usuario is null)
            return RedirectToAction("Login", "Usuario");

        var result = await userManager.DeleteAsync(usuario);

        if (result.Succeeded) 
        { 
            await signInManager.SignOutAsync();
            ApresentarMensagemSucesso($"O registro \"{registro.Nome}\" foi excluído com sucesso!");
            return RedirectToAction(nameof(Listar));
        }
        else
            ApresentarMensagemFalha("Não foi possível excluir este funcionário. Seu login continua ativo!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoFuncionario.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesFuncionarioViewModel>(registro);
        detalhesRegistroVm.Email = registro.Email;

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