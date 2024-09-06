using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class ClienteService(IRepositorioCliente repositorioCliente, IRepositorioCondutor repositorioCondutor)
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
            return Result.Fail("O cliente não foi encontrado!");

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
        => Result.Ok(repositorioCliente.Filtrar(c => c.UsuarioId == usuarioId));

    public bool SemRegistros(int? usuarioId)
        => !repositorioCliente.SelecionarTodos().Any(f => f.UsuarioId == usuarioId);

    public bool ValidarRegistroRepetido(Cliente novoRegistro, out string itemRepetido, int? usuarioId)
    {
        var cpfsCondutores = repositorioCondutor.Filtrar(c => c.UsuarioId == usuarioId).Select(c => c.CPF);
        var cnhCondutores = repositorioCondutor.Filtrar(c => c.UsuarioId == usuarioId).Select(r => r.CNH);
        
        var cpfsClientes = repositorioCliente.Filtrar(c => c.PessoaFisica && c.UsuarioId == usuarioId).Select(c => c.Documento);
        var cnhClientes = repositorioCliente.Filtrar(c => c.PessoaFisica && c.UsuarioId == usuarioId).Select(c => c.CNH);

        IEnumerable<string> cpfsExistentes = cpfsCondutores.Concat(cpfsClientes);
        IEnumerable<string> cnhExistentes = cnhCondutores.Concat(cnhClientes);
        var cnpjExistente = repositorioCliente.SelecionarTodos().FindAll(c => !c.PessoaFisica && c.UsuarioId == usuarioId).Select(c => c.Documento);
        var rgExistentes = repositorioCliente.SelecionarTodos().FindAll(c => c.PessoaFisica && c.UsuarioId == usuarioId).Select(c => c.RG);

        var registroAtual = novoRegistro.Id == 0 ? new() : repositorioCliente.SelecionarPorId(novoRegistro.Id);

        itemRepetido = "";

        if (novoRegistro.PessoaFisica)
        {
            if (cpfsExistentes.Any(c => c == novoRegistro.Documento) && novoRegistro.Documento != registroAtual!.Documento)
                itemRepetido = "cpf";

            if (cnhExistentes.Any(c => c == novoRegistro.CNH) && novoRegistro.CNH != registroAtual!.CNH)
                itemRepetido = "cnh";

            if (rgExistentes.Any(c => c == novoRegistro.RG) && novoRegistro.RG != registroAtual!.RG)
                itemRepetido = "rg";
        }
        else if (cnpjExistente.Any(c => c == novoRegistro.Documento) && novoRegistro.Documento != registroAtual!.Documento)
            itemRepetido = "cnpj";

        return itemRepetido != "";
    }

    public Result<Cliente> Desativar(int id)
    {
        var registro = repositorioCliente.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O cliente não foi encontrado!");

        registro.Ativo = false;

        repositorioCliente.Editar(registro);

        return Result.Ok();
    }
}