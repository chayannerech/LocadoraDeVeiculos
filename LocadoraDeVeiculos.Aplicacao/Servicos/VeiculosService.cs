using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using Microsoft.AspNetCore.Http;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;

public class VeiculosService(IRepositorioVeiculos repositorioVeiculos, IRepositorioGrupoDeAutomoveis repositorioGrupo)
{
    public Result<Veiculos> Inserir(Veiculos registro, int grupoId)
    {
        var grupoSelecionado = repositorioGrupo
            .SelecionarPorId(grupoId);

        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");

        registro.GrupoDeAutomoveis = grupoSelecionado;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioVeiculos.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<Veiculos> Editar(Veiculos registroAtualizado, int grupoId)
    {
        var registro = repositorioVeiculos.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        var grupoSelecionado = repositorioGrupo
            .SelecionarPorId(grupoId);

        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");

        registro.GrupoDeAutomoveis = grupoSelecionado;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

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

    public Result<List<IGrouping<string, Veiculos>>> ObterVeiculosAgrupadosPorGrupo(int? usuarioId = null)
    {
        if (usuarioId is not null)
            return Result.Ok(repositorioVeiculos.ObterVeiculosAgrupadosPorGrupo(usuarioId.Value));

        return Result.Ok(repositorioVeiculos.ObterVeiculosAgrupadosPorGrupo());
    }
}