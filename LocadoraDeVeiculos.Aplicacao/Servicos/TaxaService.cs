using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class TaxaService(IRepositorioTaxa repositorioTaxa)
{
    public Result<Taxa> Inserir(Taxa registro)
    {
        repositorioTaxa.Inserir(registro); 

        return Result.Ok(registro);
    }

    public Result<Taxa> Editar(Taxa registroAtualizado)
    {
        var registro = repositorioTaxa.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("A taxa ou serviço não foi encontrada!");

        registro.Nome = registroAtualizado.Nome;
        registro.Preco = registroAtualizado.Preco;
        registro.PrecoFixo = registroAtualizado.PrecoFixo;

        repositorioTaxa.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioTaxa.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("A taxa ou serviço não foi encontrada!");

        repositorioTaxa.Excluir(registro);

        return Result.Ok();
    }

    public Result<Taxa> SelecionarPorId(int registroId)
    {
        var registro = repositorioTaxa.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("A taxa não foi encontrada!");

        return Result.Ok(registro);
    }

    public Result<List<Taxa>> SelecionarTodos(int usuarioId)
        => Result.Ok(repositorioTaxa.Filtrar(t => t.UsuarioId == usuarioId));

    public Result<List<Taxa>> SelecionarTodos()
        => Result.Ok(repositorioTaxa.SelecionarTodos());

    public bool SemRegistros(int? usuarioId)
        => !repositorioTaxa.SelecionarTodos().Any(t => t.UsuarioId == usuarioId && t.Ativo);
    
    public bool SemRegistros()
        => repositorioTaxa.SelecionarTodos().Count == 0;

    public bool ValidarRegistroRepetido(Taxa novoRegistro, int? usuarioId)
    {
        var registrosExistentes = repositorioTaxa.Filtrar(t => t.UsuarioId == usuarioId);

        var registroAtual = novoRegistro.Id == 0 ? new() { Nome = "" } : repositorioTaxa.SelecionarPorId(novoRegistro.Id);

        return registrosExistentes.Exists(r =>
            r.Nome.Validation() == novoRegistro.Nome.Validation() &&
            r.Nome.Validation() != registroAtual!.Nome.Validation());
    }

    public Result<Taxa> Desativar(int id)
    {
        var registro = repositorioTaxa.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("A taxa não foi encontrada!");
        registro.Ativo = false;

        repositorioTaxa.Editar(registro);

        return Result.Ok();
    }
}