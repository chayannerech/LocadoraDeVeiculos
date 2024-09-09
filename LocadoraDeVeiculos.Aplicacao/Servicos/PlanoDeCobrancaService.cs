using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using Microsoft.Win32;
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
            return Result.Fail("O plano não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<PlanoDeCobranca>> SelecionarTodos(int usuarioId)
        => Result.Ok(repositorioPlano.Filtrar(p => p.UsuarioId == usuarioId));

    public Result<List<PlanoDeCobranca>> SelecionarTodos()
        => Result.Ok(repositorioPlano.SelecionarTodos());

    public Result<PlanoDeCobranca> SelecionarPorGrupoId(int grupoId)
    {
        var registro = repositorioPlano.SelecionarPorGrupoId(grupoId);

        if (registro is null)
            return Result.Fail("O plano não foi encontrado!");

        return Result.Ok(registro);
    }

    public bool PlanoRelacionadoAoGrupo(GrupoDeAutomoveis registro)
        => repositorioPlano.SelecionarTodos().Any(c => c.GrupoDeAutomoveis.Id == registro.Id);

    public bool SemRegistros(int? usuarioId)
        => !repositorioPlano.SelecionarTodos().Any(p => p.UsuarioId == usuarioId);
    
    public bool SemRegistros()
        => repositorioPlano.SelecionarTodos().Count == 0;

    public bool ValidarRegistroRepetido(PlanoDeCobranca novoRegistro, int grupoId, int? usuarioId)
    {
        var registrosExistentes = repositorioPlano.Filtrar(p => p.UsuarioId == usuarioId);

        var registroAtual = novoRegistro.Id == 0 ? new() { GrupoDeAutomoveis = new() } : repositorioPlano.SelecionarPorId(novoRegistro.Id);

        return registrosExistentes.Exists(r =>
            r.GrupoDeAutomoveis.Id == grupoId &&
            r.GrupoDeAutomoveis.Id != registroAtual!.GrupoDeAutomoveis.Id);
    }

    public Result<PlanoDeCobranca> Desativar(int id)
    {
        var registro = repositorioPlano.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O plano não foi encontrado!");

        registro.Ativo = false;

        repositorioPlano.Editar(registro);

        return Result.Ok();
    }
}