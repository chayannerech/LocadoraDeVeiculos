using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class PlanoDeCobrancaService(IRepositorioPlanoDeCobranca repositorioPlano, IRepositorioGrupoDeAutomoveis repositorioGrupo)
{
    public Result<PlanoDeCobranca> Inserir(PlanoDeCobranca registro, int grupoId)
    {
        var grupoSelecionado = repositorioGrupo
            .SelecionarPorId(grupoId);

        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");

        registro.GrupoDeAutomoveis = grupoSelecionado;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioPlano.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<PlanoDeCobranca> Editar(PlanoDeCobranca registroAtualizado)
    {
        var registro = repositorioPlano.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        registro.GrupoDeAutomoveis = registroAtualizado.GrupoDeAutomoveis;
        registro.Categoria = registroAtualizado.Categoria;
        registro.PrecoDiaria = registroAtualizado.PrecoDiaria;
        registro.PrecoKm = registroAtualizado.PrecoKm;
        registro.KmDisponivel = registroAtualizado.KmDisponivel;
        registro.PrecoDiariaControlada = registroAtualizado.PrecoDiariaControlada;
        registro.PrecoExtrapolado = registroAtualizado.PrecoExtrapolado;
        registro.PrecoLivre = registroAtualizado.PrecoLivre;

        repositorioPlano.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioPlano.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        repositorioPlano.Excluir(registro);

        return Result.Ok();
    }

    public Result<PlanoDeCobranca> SelecionarPorId(int registroId)
    {
        var registro = repositorioPlano.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<PlanoDeCobranca>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioPlano
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioPlano
            .Filtrar(f => f.Id != 0);

        return Result.Ok(registros);
    }
}