using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;
public class Aluguel() : EntidadeBase
{
    #region Propriedades de Retirada
    public Condutor Condutor { get; set; }
    public Cliente Cliente { get; set; }
    public GrupoDeAutomoveis Grupo { get; set; }
    public PlanoDeCobranca Plano { get; set; }
    public CategoriaDePlanoEnum CategoriaPlano { get; set; }
    public Veiculo Veiculo { get; set; }
    public Funcionario Funcionario { get; set; }
    public decimal Entrada { get; set; } = 1000;
    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public string TaxasSelecionadasId { get; set; }
    public List<Taxa> Taxas { get; set; }
    public bool AluguelAtivo { get; set; } = true;
    public decimal ValorTotal { get; set; }
    #endregion

    #region Propriedades de Devolução
    public DateTime DataRetornoReal { get; set; } = DateTime.Now;
    public int KmInicial { get; set; }
    public int KmFinal { get; set; }
    public bool TanqueCheio { get; set; }
    public Configuracao Configuracao { get; set; }
    #endregion

    public Aluguel(Condutor condutor, Cliente cliente, GrupoDeAutomoveis grupo, PlanoDeCobranca plano, CategoriaDePlanoEnum categoriaPlano, Veiculo veiculo, int entrada, DateTime dataSaida, DateTime dataRetornoPrevista, string taxasSelecionadasId, DateTime dataRetornoReal, int kmInicial, int kmFinal) : this()
    {
        Condutor = condutor;
        Cliente = cliente;
        Grupo = grupo;
        Plano = plano;
        CategoriaPlano = categoriaPlano;
        Veiculo = veiculo;
        Entrada = entrada;
        DataSaida = dataSaida;
        DataRetornoPrevista = dataRetornoPrevista;
        TaxasSelecionadasId = taxasSelecionadasId;
        DataRetornoReal = dataRetornoReal;
        KmInicial = kmInicial;
        KmFinal = kmFinal;
    }


    public decimal CalcularValorTotalRetirada()
    {
        var valorTotal = Entrada;
        var diasPrevistos = (DataRetornoPrevista - DataSaida).Days;

        if (CategoriaPlano == CategoriaDePlanoEnum.Diário)
            valorTotal += Plano.PrecoDiaria * diasPrevistos;
        else if (CategoriaPlano == CategoriaDePlanoEnum.Controlado)
            valorTotal += Plano.PrecoDiariaControlada * diasPrevistos;
        else
            valorTotal += Plano.PrecoLivre * diasPrevistos;

        foreach (var taxa in Taxas)
            if (taxa.PrecoFixo)
                valorTotal += taxa.Preco;
            else
                valorTotal += taxa.Preco * diasPrevistos;

        return valorTotal;
    }

    public decimal CalcularValorTotalDevolucao()
    {
        var valorTotal = Entrada;
        var kmRodados = KmFinal - KmInicial;
        var diasPrevistos = (DataRetornoPrevista - DataSaida).Days;
        var diasReais = (DataRetornoReal - DataSaida).Days;

        if (CategoriaPlano == CategoriaDePlanoEnum.Diário)
        {
            valorTotal += Plano.PrecoDiaria * diasReais;
            valorTotal += Plano.PrecoKm * kmRodados;
        }
        else if (CategoriaPlano == CategoriaDePlanoEnum.Controlado)
        {
            valorTotal += Plano.PrecoDiariaControlada * diasReais;
            if (kmRodados > Plano.KmDisponivel)
                valorTotal += (kmRodados - Plano.KmDisponivel) * Plano.PrecoExtrapolado;
        }
        else
            valorTotal += Plano.PrecoLivre * diasReais;

        foreach (var taxa in Taxas)
            if (taxa.PrecoFixo)
                valorTotal += taxa.Preco;
            else
                valorTotal += taxa.Preco * diasReais;

        if (!TanqueCheio)
        {
            var capacidadeTanque = Veiculo.CapacidadeCombustivel;

            if (Veiculo.TipoCombustivel == "Gasolina")
                valorTotal += Configuracao.Gasolina * capacidadeTanque;
            else if (Veiculo.TipoCombustivel == "Etanol")
                valorTotal += Configuracao.Etanol * capacidadeTanque;
            else if (Veiculo.TipoCombustivel == "GNV")
                valorTotal += Configuracao.GNV * capacidadeTanque;
            else
                valorTotal += Configuracao.Diesel * capacidadeTanque;
        }

        if (diasReais > diasPrevistos)
            valorTotal += valorTotal * 0.1m;

        return valorTotal;
    }


    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Condutor, "Condutor");
        VerificaNulo(ref erros, Cliente, "Cliente");
        VerificaNulo(ref erros, Veiculo, "Veiculo");
        VerificaDataInferior(ref erros, DataSaida, "O veículo deve ser retirado hoje ou após o dia de hoje");
        VerificaDataInferior(ref erros, DataRetornoPrevista, DataSaida, "O veículo deve ser devolvido após a data de retirada");        

        return erros;
    }

    public override string ToString() => $"Aluguel do {Veiculo} do dia {DataSaida.ToShortDateString()} ao dia {DataRetornoPrevista.ToShortDateString()}";
}