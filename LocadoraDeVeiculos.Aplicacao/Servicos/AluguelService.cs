using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class AluguelService(
        IRepositorioAluguel repositorioAluguel, 
        IRepositorioCondutor repositorioCondutor, 
        IRepositorioCliente repositorioCliente,
        IRepositorioGrupoDeAutomoveis repositorioGrupo,
        IRepositorioVeiculo repositorioVeiculo)
{
    public Result<Aluguel> Inserir(Aluguel registro, int condutorId, int clienteId, int grupoId, int veiculoId)
    {
        var condutorSelecionado = repositorioCondutor.SelecionarPorId(condutorId);
        var clienteSelecionado = repositorioCliente.SelecionarPorId(clienteId);
        var grupoSelecionado = repositorioGrupo.SelecionarPorId(grupoId);
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(veiculoId);

        if (condutorSelecionado is null)
            return Result.Fail("O condutor não foi selecionado!");
        if (clienteSelecionado is null)
            return Result.Fail("O cliente não foi selecionado!");
        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");
        if (veiculoSelecionado is null)
            return Result.Fail("O veículo não foi selecionado!");

        registro.CondutorNome = condutorSelecionado.Nome;
        registro.ClienteNome = clienteSelecionado.Nome;
        registro.GrupoNome = grupoSelecionado.Nome;
        registro.VeiculoId = veiculoSelecionado.Id;
        registro.VeiculoPlaca = veiculoSelecionado.Placa;
        registro.Ativo = true;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioAluguel.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<Aluguel> Editar(Aluguel registroAtualizado,int condutorId, int clienteId, int grupoId, int veiculoId)
    {
        var registro = repositorioAluguel.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O aluguel não foi encontrado!");

        var condutorSelecionado = repositorioCondutor.SelecionarPorId(condutorId);
        var clienteSelecionado = repositorioCliente.SelecionarPorId(clienteId);
        var grupoSelecionado = repositorioGrupo.SelecionarPorId(grupoId);
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(veiculoId);

        if (condutorSelecionado is null)
            return Result.Fail("O condutor não foi selecionado!");
        if (clienteSelecionado is null)
            return Result.Fail("O cliente não foi selecionado!");
        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");
        if (veiculoSelecionado is null)
            return Result.Fail("O veículo não foi selecionado!");

        registro.CondutorNome = condutorSelecionado.Nome;
        registro.ClienteNome = clienteSelecionado.Nome;
        registro.GrupoNome = grupoSelecionado.Nome;
        registro.VeiculoPlaca = veiculoSelecionado.Placa;
        registro.VeiculoId = veiculoSelecionado.Id;
        registro.CategoriaPlano = registroAtualizado.CategoriaPlano;
        registro.DataSaida = registroAtualizado.DataSaida;
        registro.DataRetornoPrevista = registroAtualizado.DataRetornoPrevista;
        registro.ValorTotal = registroAtualizado.ValorTotal;
        registro.TaxasSelecionadasId = registroAtualizado.TaxasSelecionadasId;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioAluguel.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioAluguel.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O aluguel não foi encontrado!");

        repositorioAluguel.Excluir(registro);

        return Result.Ok();
    }

    public Result Devolver(int registroId, decimal valorTotal, DateTime dataDevolucaoReal)
    {
        var registro = repositorioAluguel.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O aluguel não foi encontrado!");

        registro.Ativo = false;
        registro.ValorTotal = valorTotal;
        registro.DataRetornoReal = dataDevolucaoReal;

        repositorioAluguel.Editar(registro);

        return Result.Ok();
    }

    public Result<Aluguel> SelecionarPorId(int registroId)
    {
        var registro = repositorioAluguel.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O aluguel não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Aluguel>> SelecionarTodos(int usuarioId)
    {
        var registros = repositorioAluguel
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(registros);
    }

    public void AtualizarVeiculoDoAluguel(Veiculo registro)
    {
        var alugueisRelacionados = repositorioAluguel.SelecionarTodos().FindAll(a => a.VeiculoId == registro.Id);

        foreach (var aluguel in alugueisRelacionados)
        {
            aluguel.VeiculoPlaca = registro.Placa;
            repositorioAluguel.Editar(aluguel);
        }
    }

    public void AtualizarGrupoDoAluguel(GrupoDeAutomoveis registro)
    {
        var alugueisRelacionados = repositorioAluguel.SelecionarTodos().FindAll(a => a.GrupoId == registro.Id);

        foreach (var aluguel in alugueisRelacionados)
        {
            aluguel.GrupoNome = registro.Nome;
            repositorioAluguel.Editar(aluguel);
        }
    }

    public void AtualizarClienteDoAluguel(Cliente registro)
    {
        var alugueisRelacionados = repositorioAluguel.SelecionarTodos().FindAll(a => a.ClienteId == registro.Id);

        foreach (var aluguel in alugueisRelacionados)
        {
            aluguel.ClienteNome = registro.Nome;
            repositorioAluguel.Editar(aluguel);
        }
    }

    public void AtualizarCondutorDoAluguel(Condutor registro)
    {
        var alugueisRelacionados = repositorioAluguel.SelecionarTodos().FindAll(a => a.CondutorId == registro.Id);

        foreach (var aluguel in alugueisRelacionados)
        {
            aluguel.CondutorNome = registro.Nome;
            repositorioAluguel.Editar(aluguel);
        }
    }

    public bool AluguelRelacionadoAtivo(Veiculo registro)
        => repositorioAluguel.SelecionarTodos().FindAll(a => a.VeiculoId == registro.Id).Any(a => a.Ativo);

    public bool AluguelRelacionadoAtivo(PlanoDeCobranca registro)
    => repositorioAluguel.SelecionarTodos().FindAll(a => a.GrupoId == registro.GrupoDeAutomoveis.Id).Any(a => a.Ativo);

    public bool AluguelRelacionadoAtivo(Cliente registro)
    => repositorioAluguel.SelecionarTodos().FindAll(a => a.ClienteId == registro.Id).Any(a => a.Ativo);

    public bool AluguelRelacionadoAtivo(Condutor registro)
    => repositorioAluguel.SelecionarTodos().FindAll(a => a.CondutorId == registro.Id).Any(a => a.Ativo);

    public bool AluguelRelacionadoAtivo(Taxa registro)
        => repositorioAluguel.SelecionarTodos().FindAll(a => a.TaxasSelecionadasId.Split(',').Grupoins($"{registro.Id}")).Any(a => a.Ativo);
}