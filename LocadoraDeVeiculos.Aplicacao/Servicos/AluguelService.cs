using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracaoe;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class AluguelService(
        IRepositorioAluguel repositorioAluguel, 
        IRepositorioCondutor repositorioCondutor, 
        IRepositorioCliente repositorioCliente,
        IRepositorioTaxa repositorioTaxa,
        IRepositorioGrupoDeAutomoveis repositorioGrupo,
        IRepositorioPlanoDeCobranca repositorioPlano,
        IRepositorioFuncionario repositorioFuncionario,
        IRepositorioConfiguracao repositorioConfiguracao,
        IRepositorioVeiculo repositorioVeiculo)
{
    public Result<Aluguel> Inserir(Aluguel registro, int condutorId, int clienteId, int grupoId, int veiculoId, Funcionario funcionario)
    {
        var condutorSelecionado = repositorioCondutor.SelecionarPorId(condutorId);
        var clienteSelecionado = repositorioCliente.SelecionarPorId(clienteId);
        var grupoSelecionado = repositorioGrupo.SelecionarPorId(grupoId);
        var planoSelecionado = repositorioPlano.SelecionarPorGrupoId(grupoId);
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(veiculoId);
        var configuracaoAtiva = repositorioConfiguracao.Selecionar(registro.UsuarioId);

        if (condutorSelecionado is null)
            return Result.Fail("O condutor não foi selecionado!");
        if (clienteSelecionado is null)
            return Result.Fail("O cliente não foi selecionado!");
        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");
        if (veiculoSelecionado is null)
            return Result.Fail("O veículo não foi selecionado!");
        if (planoSelecionado is null)
            return Result.Fail("O plano não foi selecionado!");
        if (configuracaoAtiva is null)
            return Result.Fail("A configuração não foi selecionada!");

        registro.Condutor = condutorSelecionado;
        registro.Cliente = clienteSelecionado;
        registro.Grupo = grupoSelecionado;
        registro.Veiculo = veiculoSelecionado;
        registro.Plano = planoSelecionado;
        registro.Configuracao = configuracaoAtiva;
        registro.Funcionario = funcionario;
        registro.KmInicial = veiculoSelecionado.KmRodados;

        List<Taxa> taxas = [];

        if (registro.TaxasSelecionadasId != "")
            foreach (var taxaId in registro.TaxasSelecionadasId.Split(','))
                taxas.Add(repositorioTaxa.SelecionarPorId(Convert.ToInt32(taxaId))!);

        registro.Taxas = taxas;
        registro.ValorTotal = registro.CalcularValorTotalRetirada();

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
        var planoSelecionado = repositorioPlano.SelecionarPorGrupoId(grupoId);
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(veiculoId);

        if (condutorSelecionado is null)
            return Result.Fail("O condutor não foi selecionado!");
        if (clienteSelecionado is null)
            return Result.Fail("O cliente não foi selecionado!");
        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");
        if (veiculoSelecionado is null)
            return Result.Fail("O veículo não foi selecionado!");
        if (planoSelecionado is null)
            return Result.Fail("O plano não foi selecionado!");

        registro.Condutor = condutorSelecionado;
        registro.Cliente = clienteSelecionado;
        registro.Grupo = grupoSelecionado;
        registro.Veiculo = veiculoSelecionado;
        registro.Plano = planoSelecionado;
        registro.CategoriaPlano = registroAtualizado.CategoriaPlano;
        registro.DataSaida = registroAtualizado.DataSaida;
        registro.DataRetornoPrevista = registroAtualizado.DataRetornoPrevista;
        registro.TaxasSelecionadasId = registroAtualizado.TaxasSelecionadasId;
        registro.KmInicial = veiculoSelecionado.KmRodados;

        List<Taxa> taxas = [];

        if (registro.TaxasSelecionadasId != "")
            foreach (var taxaId in registro.TaxasSelecionadasId.Split(','))
                taxas.Add(repositorioTaxa.SelecionarPorId(Convert.ToInt32(taxaId))!);

        registro.Taxas = taxas;

        registro.ValorTotal = registro.CalcularValorTotalRetirada();

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

        registro.AluguelAtivo = false;
        registro.ValorTotal = valorTotal;
        registro.DataRetornoReal = dataDevolucaoReal;

        List<Taxa> taxas = [];

        if (registro.TaxasSelecionadasId != "")
            foreach (var taxaId in registro.TaxasSelecionadasId.Split(','))
                taxas.Add(repositorioTaxa.SelecionarPorId(Convert.ToInt32(taxaId))!);

        registro.Taxas = taxas;

        registro.ValorTotal = registro.CalcularValorTotalDevolucao();

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
        => Result.Ok(repositorioAluguel.Filtrar(f => f.UsuarioId == usuarioId));

    public bool AluguelRelacionadoAtivo(Veiculo registro)
        => repositorioAluguel.SelecionarTodos().Any(a => a.Veiculo.Id == registro.Id && a.AluguelAtivo);

    public bool AluguelRelacionadoAtivo(PlanoDeCobranca registro)
    => repositorioAluguel.SelecionarTodos().Any(a => a.Grupo.Id == registro.GrupoDeAutomoveis.Id && a.AluguelAtivo);

    public bool AluguelRelacionadoAtivo(Cliente registro)
    => repositorioAluguel.SelecionarTodos().Any(a => a.Cliente.Id == registro.Id && a.AluguelAtivo);

    public bool AluguelRelacionadoAtivo(Condutor registro)
    => repositorioAluguel.SelecionarTodos().Any(a => a.Condutor.Id == registro.Id && a.AluguelAtivo);

    public bool AluguelRelacionadoAtivo(Funcionario registro)
    => repositorioAluguel.SelecionarTodos().Any(a => a.Funcionario.Id == registro.Id && a.AluguelAtivo);

    public bool AluguelRelacionadoAtivo(Taxa registro)
        => repositorioAluguel.SelecionarTodos().Any(a => a.TaxasSelecionadasId.Split(',').Contains($"{registro.Id}"));

    public Result<Aluguel> Desativar(int id)
    {
        var registro = repositorioAluguel.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O cliente não foi encontrado!");

        registro.Ativo = false;

        repositorioAluguel.Editar(registro);

        return Result.Ok();
    }

    public bool SemRegistros(int usuarioId)
        => !repositorioAluguel.SelecionarTodos().Any(f => f.UsuarioId == usuarioId);
}