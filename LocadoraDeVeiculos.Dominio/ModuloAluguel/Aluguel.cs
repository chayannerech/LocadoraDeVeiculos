using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;
public class Aluguel() : EntidadeBase
{
    public string CondutorNome { get; set; }
    public int CondutorId { get; set; }

    public string ClienteNome { get; set; }
    public int ClienteId { get; set; }

    public string GrupoNome { get; set; }
    public int GrupoId { get; set; }

    public CategoriaDePlanoEnum CategoriaPlano { get; set; }

    public int VeiculoId { get; set; }
    public string VeiculoPlaca { get; set; }

    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public DateTime DataRetornoReal { get; set; }
    public string TaxasSelecionadasId { get; set; }
    public decimal ValorTotal { get; set; }
    public bool Ativo { get; set; }

    public Aluguel(int condutorId, int clienteId, int grupoId, int veiculoId, DateTime dataSaida, DateTime dataRetornoPrevista, DateTime dataRetornoReal) : this()
    {
        CondutorId = condutorId;
        ClienteId = clienteId;
        GrupoId = grupoId;
        VeiculoId = veiculoId;
        DataSaida = dataSaida;
        DataRetornoPrevista = dataRetornoPrevista;
        DataRetornoReal = dataRetornoReal;
    }

    public decimal CalcularValorTotalRetirada(GrupoDeAutomoveis grupo, CategoriaDePlanoEnum categoria, List<Taxa> taxas, int diasDeAluguel, decimal entrada)
    {
        var valorTotal = entrada;

        if (categoria == CategoriaDePlanoEnum.Diário)
            valorTotal += grupo.PrecoDiaria * diasDeAluguel;
        else if (categoria == CategoriaDePlanoEnum.Controlado)
            valorTotal += grupo.PrecoDiariaControlada * diasDeAluguel;
        else
            valorTotal += grupo.PrecoLivre * diasDeAluguel;

        foreach (var taxa in taxas)
            if (taxa.PrecoFixo)
                valorTotal += taxa.Preco;
            else
                valorTotal += taxa.Preco * diasDeAluguel;

        return valorTotal;
    }

    public decimal CalcularValorTotalDevolucao(
        PlanoDeCobranca plano, 
        CategoriaDePlanoEnum categoria,
        List<Taxa> taxas, int diasPlanejados, 
        decimal entrada,
        int diasReais, 
        int kmInicial, 
        int kmAtual, 
        bool tanqueCheio,
        Veiculo veiculo,
        Configuracao configuracao)
    {
        var valorTotal = entrada;
        var km = kmAtual - kmInicial;

        if (categoria == CategoriaDePlanoEnum.Diário)
        {
            valorTotal += plano.PrecoDiaria * diasReais;
            valorTotal += plano.PrecoKm * km;
        }
        else if (categoria == CategoriaDePlanoEnum.Controlado)
        {
            valorTotal += plano.PrecoDiariaControlada * diasReais;
            if (km > plano.KmDisponivel)
                valorTotal += (km - plano.KmDisponivel) * plano.PrecoExtrapolado;
        }
        else
            valorTotal += plano.PrecoLivre * diasReais;

        foreach (var taxa in taxas)
            if (taxa.PrecoFixo)
                valorTotal += taxa.Preco;
            else
                valorTotal += taxa.Preco * diasReais;

        if (!tanqueCheio)
        {
            var capacidadeTanque = veiculo.CapacidadeCombustivel;

            if (veiculo.TipoCombustivel == "Gasolina")
                valorTotal += configuracao.Gasolina * capacidadeTanque;
            else if (veiculo.TipoCombustivel == "Etanol")
                valorTotal += configuracao.Etanol * capacidadeTanque;
            else if (veiculo.TipoCombustivel == "GNV")
                valorTotal += configuracao.GNV * capacidadeTanque;
            else
                valorTotal += configuracao.Diesel * capacidadeTanque;
        }

        if (diasReais > diasPlanejados)
            valorTotal += valorTotal * 0.1m;

        return valorTotal;
    }


    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, CondutorId, "Condutor");
        VerificaNulo(ref erros, ClienteId, "Cliente");
        VerificaNulo(ref erros, VeiculoId, "Veiculo");
        VerificaDataInferior(ref erros, DataSaida, "O veículo deve ser retirado hoje ou após o dia de hoje");
        VerificaDataInferior(ref erros, DataRetornoPrevista, DataSaida, "O veículo deve ser devolvido após a data de retirada");        

        if (DataRetornoReal > DateTime.MinValue)
            VerificaDataInferior(ref erros, DataRetornoReal, DataSaida, "O veículo deve ser devolvido após a data de retirada");

        return erros;
    }

    public override string ToString() => $"Aluguel do veículo placa {VeiculoPlaca} do dia {DataSaida.ToShortDateString()} ao dia {DataRetornoPrevista.ToShortDateString()}";
}