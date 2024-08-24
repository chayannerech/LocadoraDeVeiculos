using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
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
            return Result.Fail("A taxa ou serviço não foi encontrada!");

        return Result.Ok(registro);
    }

    public Result<List<Taxa>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioTaxa
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioTaxa
            .Filtrar(f => f.Id != 0);

        return Result.Ok(registros);
    }
}