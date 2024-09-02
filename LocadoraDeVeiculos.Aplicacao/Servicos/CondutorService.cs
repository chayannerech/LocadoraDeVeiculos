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
    {
        var registros = repositorioCondutor
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(registros);
    }

    public bool CondutorRelacionado(Cliente registro)
        => repositorioCondutor.SelecionarTodos().Any(c => c.Cliente.Id == registro.Id);

    public bool SemRegistros()
        => repositorioCondutor.SelecionarTodos().Count == 0;

    public bool ValidarRegistroRepetido(bool check, Condutor novoRegistro, out string itemRepetido)
    {
        var cpfsCondutores = repositorioCondutor.SelecionarTodos().Select(r => r.CPF);
        var cnhCondutores = repositorioCondutor.SelecionarTodos().Select(r => r.CNH);

        var cpfsClientes = repositorioCliente.SelecionarTodos().FindAll(c => c.PessoaFisica).Select(c => c.Documento);
        var cnhClientes = repositorioCliente.SelecionarTodos().FindAll(c => c.PessoaFisica).Select(c => c.CNH);

        IEnumerable<string> cpfsExistentes = cpfsCondutores;
        IEnumerable<string> cnhExistentes = cnhCondutores;

        if (!check)
        {
            cpfsExistentes = cpfsExistentes.Concat(cpfsClientes);
            cnhExistentes = cnhCondutores.Concat(cnhClientes);
        }

        var registroAtual = novoRegistro.Id == 0 ? new() : repositorioCondutor.SelecionarPorId(novoRegistro.Id);

        if (cpfsExistentes.Any(c => c.Equals(novoRegistro.CPF)) && novoRegistro.CPF != registroAtual!.CPF)
        {
            itemRepetido = "cpf";
            return true;
        }

        if (cnhExistentes.Any(c => c.Equals(novoRegistro.CNH)) && novoRegistro.CNH != registroAtual!.CNH)
        {
            itemRepetido = "cnh";
            return true;
        }

        itemRepetido = "";
        return false;
    }
}