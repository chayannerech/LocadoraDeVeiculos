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

    #region CRUD
    public Result<Aluguel> Inserir(Aluguel registro, int condutorId, int clienteId, int grupoId, int veiculoId, Funcionario funcionario)
    {
        var condutorSelecionado = repositorioCondutor.SelecionarPorId(condutorId);
        var clienteSelecionado = repositorioCliente.SelecionarPorId(clienteId);
        var grupoSelecionado = repositorioGrupo.SelecionarPorId(grupoId);
        var planoSelecionado = repositorioPlano.SelecionarPorGrupoId(grupoId);
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(veiculoId);
        var configuracaoAtiva = repositorioConfiguracao.Selecionar(registro.UsuarioId);

        Result<Aluguel> resultado = TestarDependenciasEncontradas(condutorSelecionado, clienteSelecionado, grupoSelecionado, planoSelecionado, veiculoSelecionado);
        if (resultado.IsFailed) return resultado;

        registro.Condutor = condutorSelecionado!;
        registro.Cliente = clienteSelecionado!;
        registro.Grupo = grupoSelecionado!;
        registro.Veiculo = veiculoSelecionado!;
        registro.Plano = planoSelecionado;
        registro.Configuracao = configuracaoAtiva;
        registro.Funcionario = funcionario;
        registro.KmInicial = veiculoSelecionado!.KmRodados;
        registro.Taxas = ListarTaxas(repositorioTaxa, registro);
        registro.ValorTotal = registro.CalcularValorTotalRetirada();

        return ValidarEntidadeESubmeter(repositorioAluguel, registro,
            () => repositorioAluguel.Inserir(registro));
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

        Result<Aluguel> resultado = TestarDependenciasEncontradas(condutorSelecionado, clienteSelecionado, grupoSelecionado, planoSelecionado, veiculoSelecionado);
        if (resultado.IsFailed) return resultado;

        registro.Condutor = condutorSelecionado!;
        registro.Cliente = clienteSelecionado!;
        registro.Grupo = grupoSelecionado!;
        registro.Veiculo = veiculoSelecionado!;
        registro.Plano = planoSelecionado;
        registro.CategoriaPlano = registroAtualizado.CategoriaPlano;
        registro.DataSaida = registroAtualizado.DataSaida;
        registro.DataRetornoPrevista = registroAtualizado.DataRetornoPrevista;
        registro.TaxasSelecionadasId = registroAtualizado.TaxasSelecionadasId;
        registro.KmInicial = veiculoSelecionado!.KmRodados;
        registro.Taxas = ListarTaxas(repositorioTaxa, registro);
        registro.ValorTotal = registro.CalcularValorTotalRetirada();

        return ValidarEntidadeESubmeter(repositorioAluguel, registro,
            () => repositorioAluguel.Editar(registro));
    }

    public Result<Aluguel> Desativar(int id)
    {
        var registro = repositorioAluguel.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O cliente não foi encontrado!");

        registro.Ativo = false;

        repositorioAluguel.Editar(registro);

        return Result.Ok();
    }
    #endregion

    #region Ações auxiliares
    public Result<Aluguel> SelecionarPorId(int registroId)
    {
        var registro = repositorioAluguel.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O aluguel não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Aluguel>> SelecionarTodos(int usuarioId)
        => Result.Ok(repositorioAluguel.Filtrar(f => f.UsuarioId == usuarioId));

    public Result Devolver(Aluguel registro, DateTime dataDevolucaoReal, bool tanqueCheio)
    {
        registro.AluguelAtivo = false;
        registro.DataRetornoReal = dataDevolucaoReal;
        registro.TanqueCheio = tanqueCheio;

        registro.Taxas = ListarTaxas(repositorioTaxa, registro);

        registro.ValorTotal = registro.CalcularValorTotalDevolucao();

        repositorioAluguel.Editar(registro);

        return Result.Ok();
    }

    private static Result<Aluguel> ValidarEntidadeESubmeter(IRepositorioAluguel repositorioAluguel, Aluguel registro, Action acao)
    {
        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        acao();

        return Result.Ok(registro);
    }

    private static List<Taxa> ListarTaxas(IRepositorioTaxa repositorioTaxa, Aluguel? registro)
    {
        List<Taxa> taxas = [];

        if (registro.TaxasSelecionadasId != "")
            foreach (var taxaId in registro.TaxasSelecionadasId.Split(','))
                taxas.Add(repositorioTaxa.SelecionarPorId(Convert.ToInt32(taxaId))!);
        return taxas;
    }

    private static Result<Aluguel> TestarDependenciasEncontradas(Condutor? condutorSelecionado, Cliente? clienteSelecionado, GrupoDeAutomoveis? grupoSelecionado, PlanoDeCobranca? planoSelecionado, Veiculo? veiculoSelecionado)
    {
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

        return Result.Ok();
    }

    #endregion

    #region Validações
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
        => repositorioAluguel.SelecionarTodos().Any(a => a.TaxasSelecionadasId.Split(',').Contains($"{registro.Id}") && a.AluguelAtivo);
    public bool SemRegistros(int usuarioId)
        => !repositorioAluguel.SelecionarTodos().Any(f => f.UsuarioId == usuarioId);
    #endregion
}