using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using System.Security.Claims;

namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class FuncionarioService(IRepositorioFuncionario repositorioFuncionario)
{
    public Result<Funcionario> Inserir(Funcionario registro)
    {
        repositorioFuncionario.Inserir(registro); 

        return Result.Ok(registro);
    }

    public Result<Funcionario> Editar(Funcionario registroAtualizado)
    {
        var registro = repositorioFuncionario.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O funcionário não foi encontrado!");

        registro.Nome = registroAtualizado.Nome;
        registro.Login = registroAtualizado.Login;
        registro.DataAdmissao = registroAtualizado.DataAdmissao;
        registro.Salario = registroAtualizado.Salario;

        repositorioFuncionario.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioFuncionario.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O funcionário não foi encontrado!");

        repositorioFuncionario.Excluir(registro);

        return Result.Ok();
    }

    public Result<Funcionario> SelecionarPorId(int registroId)
    {
        var registro = repositorioFuncionario.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O funcionário não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<Funcionario> SelecionarPorLogin(string registroLogin)
    {
        var registro = repositorioFuncionario.SelecionarTodos().Find(f => f.Login == registroLogin);

        if (registro is null)
            return Result.Fail("O funcionário não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Funcionario>> SelecionarTodos(int usuarioId)
        => Result.Ok(repositorioFuncionario.Filtrar(f => f.UsuarioId == usuarioId && f.Nome != "Empresa"));

    public bool SemRegistros(int? usuarioId)
        => !repositorioFuncionario.SelecionarTodos().Any(f => f.UsuarioId == usuarioId);

    public bool ValidarRegistroRepetido(Funcionario novoRegistro, int? usuarioId)
    {
        var registrosExistentes = repositorioFuncionario.Filtrar(f => f.UsuarioId == usuarioId);
        var registroAtual = novoRegistro.Id == 0 ? new() { Nome = "" } : repositorioFuncionario.SelecionarPorId(novoRegistro.Id)!;

        return registrosExistentes.Exists(r =>
            r.Login == novoRegistro.Login &&
            r.Login != registroAtual.Login);
    }

    public Result<Funcionario> Desativar(int id)
    {
        var registro = repositorioFuncionario.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O funcionário não foi encontrado!");

        registro.Ativo = false;

        repositorioFuncionario.Editar(registro);

        return Result.Ok();
    }

    public Funcionario AtribuirFuncionarioAoAluguel(ClaimsPrincipal user, int usuarioId)
    {
        Funcionario funcionario;

        var userLogin = user
            .FindFirst(ClaimTypes.NameIdentifier)!
            .Subject!
            .Name!.ToString();

        if (user.IsInRole("Funcionario"))
            funcionario = SelecionarPorLogin(userLogin).Value;
        else
        {
            if (SelecionarPorLogin(userLogin).IsFailed)
            {
                funcionario = new($"Empresa", DateTime.Now, 10, userLogin);
                funcionario.UsuarioId = usuarioId;
            }
            else
                funcionario = SelecionarPorLogin(userLogin).Value;
        }

        return funcionario;
    }
}