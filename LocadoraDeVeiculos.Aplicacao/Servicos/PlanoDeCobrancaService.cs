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

    public Result<PlanoDeCobranca> Editar(PlanoDeCobranca registroAtualizado, int grupoId)
    {
        var registro = repositorioPlano.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O plano de cobrança não foi encontrado!");

        var grupoSelecionado = repositorioGrupo.SelecionarPorId(grupoId);

        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");

        registro.PrecoDiaria = registroAtualizado.PrecoDiaria;
        registro.PrecoKm = registroAtualizado.PrecoKm;
        registro.KmDisponivel = registroAtualizado.KmDisponivel;
        registro.PrecoDiariaControlada = registroAtualizado.PrecoDiariaControlada;
        registro.PrecoExtrapolado = registroAtualizado.PrecoExtrapolado;
        registro.PrecoLivre = registroAtualizado.PrecoLivre;
        registro.GrupoDeAutomoveis = grupoSelecionado;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioPlano.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioPlano.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O plano de cobrança não foi encontrado!");

        repositorioPlano.Excluir(registro);

        return Result.Ok();
    }

    public Result<PlanoDeCobranca> SelecionarPorId(int registroId)
    {
        var registro = repositorioPlano.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O plano de cobrança não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<PlanoDeCobranca>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioPlano
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioPlano
            .SelecionarTodos();

        return Result.Ok(registros);
    }
}