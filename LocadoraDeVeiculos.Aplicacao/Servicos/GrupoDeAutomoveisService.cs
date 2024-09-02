using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class GrupoDeAutomoveisService(IRepositorioGrupoDeAutomoveis repositorioGrupo)
{
    public Result<GrupoDeAutomoveis> Inserir(GrupoDeAutomoveis registro)
    {
        repositorioGrupo.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<GrupoDeAutomoveis> Editar(GrupoDeAutomoveis registroAtualizado)
    {
        var registro = repositorioGrupo.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        registro.Nome = registroAtualizado.Nome;

        repositorioGrupo.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioGrupo.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        repositorioGrupo.Excluir(registro);

        return Result.Ok();
    }

    public Result<GrupoDeAutomoveis> SelecionarPorId(int registroId)
    {
        var registro = repositorioGrupo.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O grupo de automóveis não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<GrupoDeAutomoveis>> SelecionarTodos(int usuarioId)
    {
        var registros = repositorioGrupo
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(registros);
    }

    public void AdicionarValores(PlanoDeCobranca plano)
    {
        var grupoSelecionado = plano.GrupoDeAutomoveis;

        grupoSelecionado.PrecoDiaria = plano.PrecoDiaria;
        grupoSelecionado.PrecoDiariaControlada = plano.PrecoDiariaControlada;
        grupoSelecionado.PrecoLivre = plano.PrecoLivre;

        repositorioGrupo.Editar(grupoSelecionado);
    }

    public void ExcluirValores(PlanoDeCobranca plano)
    {
        var grupoSelecionado = plano.GrupoDeAutomoveis;

        grupoSelecionado.PrecoDiaria = 0;
        grupoSelecionado.PrecoDiariaControlada = 0;
        grupoSelecionado.PrecoLivre = 0;

        repositorioGrupo.Editar(grupoSelecionado);
    }

    public bool ValidarRegistroRepetido(GrupoDeAutomoveis novoRegistro)
    {
        var registrosExistentes = repositorioGrupo.SelecionarTodos();
        var registroAtual = novoRegistro.Id == 0 ? new() { Nome = "" } : repositorioGrupo.SelecionarPorId(novoRegistro.Id)!;

        return registrosExistentes.Exists(r =>
            r.Nome.Validation() == novoRegistro.Nome.Validation() &&
            r.Nome.Validation() != registroAtual.Nome.Validation());
    }

    public bool SemRegistros()
        => repositorioGrupo.SelecionarTodos().Count == 0;
}