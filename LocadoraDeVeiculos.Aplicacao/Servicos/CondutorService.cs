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
            return Result.Fail("O condutor não foi encontrado!");

        repositorioCondutor.Excluir(registro);

        return Result.Ok();
    }

    public Result<Condutor> SelecionarPorId(int registroId)
    {
        var registro = repositorioCondutor.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O condutor não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Condutor>> SelecionarTodos(int usuarioId)
        => Result.Ok(repositorioCondutor.Filtrar(c => c.UsuarioId == usuarioId));

    public bool CondutorRelacionado(Cliente registro)
        => repositorioCondutor.SelecionarTodos().Any(c => c.Cliente.Id == registro.Id);

    public bool SemRegistros(int? usuarioId)
        => !repositorioCondutor.SelecionarTodos().Any(f => f.UsuarioId == usuarioId);

    public bool ValidarRegistroRepetido(bool check, Condutor novoRegistro, out string itemRepetido, int? usuarioId)
    {
        var cpfsCondutores = repositorioCondutor.Filtrar(c => c.UsuarioId == usuarioId).Select(c => c.CPF);
        var cnhCondutores = repositorioCondutor.Filtrar(c => c.UsuarioId == usuarioId).Select(c => c.CNH);

        var cpfsClientes = repositorioCliente.Filtrar(c => c.PessoaFisica && c.UsuarioId == usuarioId).Select(c => c.Documento);
        var cnhClientes = repositorioCliente.Filtrar(c => c.PessoaFisica && c.UsuarioId == usuarioId).Select(c => c.CNH);

        IEnumerable<string> cpfsExistentes = cpfsCondutores;
        IEnumerable<string> cnhExistentes = cnhCondutores;

        if (!check)
        {
            cpfsExistentes = cpfsExistentes.Concat(cpfsClientes);
            cnhExistentes = cnhCondutores.Concat(cnhClientes);
        }

        var registroAtual = novoRegistro.Id == 0 ? new() : repositorioCondutor.SelecionarPorId(novoRegistro.Id);

        itemRepetido = "";

        if (cpfsExistentes.Any(c => c.Equals(novoRegistro.CPF)) && novoRegistro.CPF != registroAtual!.CPF)
            itemRepetido = "cpf";

        if (cnhExistentes.Any(c => c.Equals(novoRegistro.CNH)) && novoRegistro.CNH != registroAtual!.CNH)
            itemRepetido = "cnh";

        return itemRepetido != "";
    }

    public Result<Condutor> Desativar(int id)
    {
        var registro = repositorioCondutor.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O condutor não foi encontrado!");

        registro.Ativo = false;

        repositorioCondutor.Editar(registro);

        return Result.Ok();
    }
}