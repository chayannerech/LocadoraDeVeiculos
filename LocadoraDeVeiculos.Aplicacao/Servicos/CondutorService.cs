using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class CondutorService(IRepositorioCondutor repositorioCondutor, IRepositorioCliente repositorioCliente)
{
    public Result<Condutor> Inserir(Condutor registro, int clienteId)
    {
        var clienteSelecionado = repositorioCliente
            .SelecionarPorId(clienteId);

        if (clienteSelecionado is null)
            return Result.Fail("O cliente não foi selecionado!");

        registro.Cliente = clienteSelecionado;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioCondutor.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<Condutor> Editar(Condutor registroAtualizado, int clienteId)
    {
        var registro = repositorioCondutor.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O condutor não foi encontrado!");

        var clienteSelecionado = repositorioCliente.SelecionarPorId(clienteId);

        if (clienteSelecionado is null)
            return Result.Fail("O cliente não foi selecionado!");

        registro.Cliente = clienteSelecionado;
        registro.Nome = registroAtualizado.Nome;
        registro.Email = registroAtualizado.Email;
        registro.Telefone = registroAtualizado.Telefone;
        registro.CPF = registroAtualizado.CPF;
        registro.ValidadeCNH = registroAtualizado.ValidadeCNH;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioCondutor.Editar(registro);

        return Result.Ok(registro);
    }


    public Result Excluir(int registroId)
    {
        var registro = repositorioCondutor.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O cliente não foi encontrado!");

        repositorioCondutor.Excluir(registro);

        return Result.Ok();
    }

    public Result<Condutor> SelecionarPorId(int registroId)
    {
        var registro = repositorioCondutor.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O cliente não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Condutor>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioCondutor
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioCondutor
            .Filtrar(f => f.Id != 0);

        return Result.Ok(registros);
    }
}