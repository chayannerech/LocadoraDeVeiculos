using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;

namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class GrupoDeAutomoveisService(IRepositorioGrupoDeAutomoveis repositorioGrupoDeAutomoveis)
{
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

    public void AdicionarValores(PlanoDeCobranca plano)
    {
        var grupoSelecionado = plano.GrupoDeAutomoveis;

        grupoSelecionado.PrecoDiaria = plano.PrecoDiaria;
        grupoSelecionado.PrecoDiariaControlada = plano.PrecoDiariaControlada;
        grupoSelecionado.PrecoLivre = plano.PrecoLivre;

        repositorioGrupoDeAutomoveis.Editar(grupoSelecionado);
    }

    public void ExcluirValores(PlanoDeCobranca plano)
    {
        var grupoSelecionado = plano.GrupoDeAutomoveis;

        grupoSelecionado.PrecoDiaria = 0;
        grupoSelecionado.PrecoDiariaControlada = 0;
        grupoSelecionado.PrecoLivre = 0;

        repositorioGrupoDeAutomoveis.Editar(grupoSelecionado);
    }
}