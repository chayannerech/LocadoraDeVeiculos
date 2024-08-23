using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class PlanoDeCobrancaService(IRepositorioPlanoDeCobranca repositorioPlanoDeCobranca)
{
    private readonly IRepositorioPlanoDeCobranca repositorioPlanoDeCobranca = repositorioPlanoDeCobranca;

    public Result<PlanoDeCobranca> Inserir(PlanoDeCobranca registro)
    {
        repositorioPlanoDeCobranca.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<PlanoDeCobranca> Editar(PlanoDeCobranca registroAtualizado)
    {
        var registro = repositorioPlanoDeCobranca.SelecionarPorId(registroAtualizado.Id);

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

        repositorioPlanoDeCobranca.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioPlanoDeCobranca.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        repositorioPlanoDeCobranca.Excluir(registro);

        return Result.Ok();
    }

    public Result<PlanoDeCobranca> SelecionarPorId(int registroId)
    {
        var registro = repositorioPlanoDeCobranca.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<PlanoDeCobranca>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioPlanoDeCobranca
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioPlanoDeCobranca
            .Filtrar(f => f.Id != 0);

        return Result.Ok(registros);
    }
}