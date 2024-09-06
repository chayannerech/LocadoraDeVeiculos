using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
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
            return Result.Fail("O grupo não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<GrupoDeAutomoveis>> SelecionarTodos(int usuarioId)
        => Result.Ok(repositorioGrupo.Filtrar(g => g.UsuarioId == usuarioId));

    public Result<List<GrupoDeAutomoveis>> SelecionarTodos()
        => Result.Ok(repositorioGrupo.Filtrar(g => g.Ativo));

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

    public bool ValidarRegistroRepetido(GrupoDeAutomoveis novoRegistro, int usuarioId)
    {
        var registrosExistentes = repositorioGrupo.Filtrar(g => g.UsuarioId == usuarioId);
        var registroAtual = novoRegistro.Id == 0 ? new() { Nome = "" } : repositorioGrupo.SelecionarPorId(novoRegistro.Id)!;

        return registrosExistentes.Exists(r =>
            r.Nome.Validation() == novoRegistro.Nome.Validation() &&
            r.Nome.Validation() != registroAtual.Nome.Validation());
    }

    public bool SemRegistros(int? usuarioId)
        => !repositorioGrupo.SelecionarTodos().Any(f => f.UsuarioId == usuarioId);

    public bool SemRegistros()
        => repositorioGrupo.SelecionarTodos().Count == 0;

    public Result<GrupoDeAutomoveis> Desativar(int id)
    {
        var registro = repositorioGrupo.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O grupo não foi encontrado!");
        registro.Ativo = false;

        repositorioGrupo.Editar(registro);

        return Result.Ok();
    }
}