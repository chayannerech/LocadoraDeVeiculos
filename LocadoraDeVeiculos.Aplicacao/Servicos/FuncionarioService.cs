using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
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
        registro.Senha = registroAtualizado.Senha;

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

    public Result<List<Funcionario>> SelecionarTodos(int usuarioId)
    {
        var registros = repositorioFuncionario
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(registros);
    }

    public bool SemRegistros()
        => repositorioFuncionario.SelecionarTodos().Count == 0;

    public bool ValidarRegistroRepetido(Funcionario novoRegistro)
    {
        var registrosExistentes = repositorioFuncionario.SelecionarTodos();
        var registroAtual = novoRegistro.Id == 0 ? new() { Login = "" } : repositorioFuncionario.SelecionarPorId(novoRegistro.Id)!;

        return registrosExistentes.Exists(r =>
            r.Login.Validation() == novoRegistro.Login.Validation() &&
            r.Login.Validation() != registroAtual.Login.Validation());
    }
}