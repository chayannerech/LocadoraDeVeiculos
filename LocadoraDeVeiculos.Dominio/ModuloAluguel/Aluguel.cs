using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
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

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, CondutorId, "Condutor");
        VerificaNulo(ref erros, ClienteId, "Cliente");
        VerificaNulo(ref erros, VeiculoId, "Veiculo");
        VerificaDataInferior(ref erros, DataSaida, "O veículo deve ser retirado hoje ou após o dia de hoje");
        VerificaDataInferior(ref erros, DataRetornoPrevista, DataSaida, "O veículo deve ser devolvido após a data de retirada");        

        if (DataRetornoPrevista > DateTime.MinValue)
            VerificaDataInferior(ref erros, DataRetornoReal, DataSaida, "O veículo deve ser devolvido após a data de retirada");

        return erros;
    }

    public override string ToString() => $"Aluguel do veículo placa {VeiculoPlaca} do dia {DataSaida.ToShortDateString()} ao dia {DataRetornoPrevista.ToShortDateString()}";
}