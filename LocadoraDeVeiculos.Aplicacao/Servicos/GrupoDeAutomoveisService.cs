using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class GrupoDeAutomoveisService(IRepositorioGrupoDeAutomoveis repositorioGrupoDeAutomoveis)
{
    private readonly IRepositorioGrupoDeAutomoveis repositorioGrupoDeAutomoveis = repositorioGrupoDeAutomoveis;

    public Result<GrupoDeAutomoveis> Inserir(GrupoDeAutomoveis registro)
    {
        repositorioGrupoDeAutomoveis.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<GrupoDeAutomoveis> Editar(GrupoDeAutomoveis registroAtualizado)
    {
        var registro = repositorioGrupoDeAutomoveis.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        registro.Nome = registroAtualizado.Nome;

        repositorioGrupoDeAutomoveis.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioGrupoDeAutomoveis.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        repositorioGrupoDeAutomoveis.Excluir(registro);

        return Result.Ok();
    }

    public Result<GrupoDeAutomoveis> SelecionarPorId(int registroId)
    {
        var registro = repositorioGrupoDeAutomoveis.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<GrupoDeAutomoveis>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioGrupoDeAutomoveis
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioGrupoDeAutomoveis
            .Filtrar(f => f.Id != 0);

        return Result.Ok(registros);
    }
}