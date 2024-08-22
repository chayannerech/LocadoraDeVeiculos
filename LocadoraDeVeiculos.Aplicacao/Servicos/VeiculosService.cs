using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;

public class VeiculosService(IRepositorioVeiculos repositorioVeiculos)
{
    private readonly IRepositorioVeiculos repositorioVeiculos = repositorioVeiculos;

    public Result<Veiculos> Inserir(Veiculos registro)
    {
        repositorioVeiculos.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<Veiculos> Editar(Veiculos registroAtualizado)
    {
        var registro = repositorioVeiculos.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        registro.Nome = registroAtualizado.Nome;

        repositorioVeiculos.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioVeiculos.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        repositorioVeiculos.Excluir(registro);

        return Result.Ok();
    }

    public Result<Veiculos> SelecionarPorId(int registroId)
    {
        var registro = repositorioVeiculos.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Veiculos>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioVeiculos
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioVeiculos
            .Filtrar(f => f.Id != 0);

        return Result.Ok(registros);
    }
}