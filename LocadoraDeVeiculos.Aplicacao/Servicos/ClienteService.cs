using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class ClienteService(IRepositorioCliente repositorioCliente)
{
    public Result<Cliente> Inserir(Cliente registro)
    {
        repositorioCliente.Inserir(registro); 

        return Result.Ok(registro);
    }

    public Result<Cliente> Editar(Cliente registroAtualizado)
    {
        var registro = repositorioCliente.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("A taxa ou serviço não foi encontrada!");

        registro.Nome = registroAtualizado.Nome;
        registro.Email = registroAtualizado.Email;
        registro.Telefone = registroAtualizado.Telefone;
        registro.PessoaFisica = registroAtualizado.PessoaFisica;
        registro.Documento = registroAtualizado.Documento;
        registro.Cidade = registroAtualizado.Cidade;
        registro.Estado = registroAtualizado.Estado;
        registro.Bairro = registroAtualizado.Bairro;
        registro.Rua = registroAtualizado.Rua;
        registro.Numero = registroAtualizado.Numero;

        repositorioCliente.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioCliente.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O cliente não foi encontrado!");

        repositorioCliente.Excluir(registro);

        return Result.Ok();
    }

    public Result<Cliente> SelecionarPorId(int registroId)
    {
        var registro = repositorioCliente.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O cliente não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Cliente>> SelecionarTodos(int usuarioId)
    {
        /*        var registros = repositorioCliente
                    .Filtrar(f => f.UsuarioId == usuarioId);

                return Result.Ok(registros);*/

        var registros = repositorioCliente
            .Filtrar(f => f.Id != 0);

        return Result.Ok(registros);
    }
}