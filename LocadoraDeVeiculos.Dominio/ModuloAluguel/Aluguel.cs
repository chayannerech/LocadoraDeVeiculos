using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;
public class Aluguel() : EntidadeBase
{
    public Condutor Condutor { get; set; }
    public string CondutorNome { get => Condutor is not null ? Condutor.Nome : ""; set { } }

    public Cliente Cliente { get; set; }
    public string ClienteNome { get => Cliente is not null ? Cliente.Nome : ""; set { } }

    public PlanoDeCobranca PlanoDeCobranca { get; set; }
    public CategoriaDePlanoEnum CategoriaPlano { get; set; }

    public GrupoDeAutomoveis GrupoDeAutomoveis { get; set; }
    public string GrupoNome { get => GrupoDeAutomoveis is not null ? GrupoDeAutomoveis.Nome : ""; set { } }

    public Veiculo Veiculo { get; set; }
    public string VeiculoPlaca { get => Veiculo is not null ? Veiculo.Placa : ""; set { } }

    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public DateTime DataRetornoReal { get; set; }

    public string TaxasSelecionadasId { get; set; }

    public decimal ValorTotal { get; set; }
    public bool Ativo { get; set; }


    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Condutor, "Condutor");
        VerificaNulo(ref erros, Cliente, "Cliente");
        VerificaNulo(ref erros, PlanoDeCobranca, "PlanoDeCobranca");
        VerificaNulo(ref erros, Veiculo, "Veiculo");
        VerificaDataInferior(ref erros, DataSaida, "O veículo deve ser retirado hoje ou após o dia de hoje");
        VerificaDataInferior(ref erros, DataRetornoPrevista, DataSaida, "O veículo deve ser devolvido após a data de retirada");        

        return erros;
    }

    public override string ToString() => $"Aluguel do {Veiculo} do dia {DataSaida.ToShortDateString()} ao dia {DataRetornoPrevista.ToShortDateString()}";
}