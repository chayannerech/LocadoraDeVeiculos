using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
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


    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, CondutorNome, "Condutor");
        VerificaNulo(ref erros, ClienteNome, "Cliente");
        VerificaNulo(ref erros, VeiculoPlaca, "Veiculo");
        VerificaDataInferior(ref erros, DataSaida, "O veículo deve ser retirado hoje ou após o dia de hoje");
        VerificaDataInferior(ref erros, DataRetornoPrevista, DataSaida, "O veículo deve ser devolvido após a data de retirada");        

        return erros;
    }

    public override string ToString() => $"Aluguel do veículo placa {VeiculoPlaca} do dia {DataSaida.ToShortDateString()} ao dia {DataRetornoPrevista.ToShortDateString()}";
}