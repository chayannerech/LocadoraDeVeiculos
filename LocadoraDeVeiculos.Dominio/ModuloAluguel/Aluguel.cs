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
    public Cliente Cliente { get; set; }
    public PlanoDeCobranca PlanoDeCobranca { get; set; }
    public GrupoDeAutomoveis GrupoDeAutomoveis { get; set; }
    public Veiculo Veiculo { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime DataRetorno { get; set; }
    public bool SeguroCondutor { get; set; }
    public bool SeguroTerceiro {  get; set; }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Condutor, "Condutor");
        VerificaNulo(ref erros, Cliente, "Cliente");
        VerificaNulo(ref erros, PlanoDeCobranca, "PlanoDeCobranca");
        VerificaNulo(ref erros, Veiculo, "Veiculo");
        VerificaDataInferior(ref erros, DataSaida, "O veículo deve ser retirado hoje ou após o dia de hoje");
        VerificaDataInferior(ref erros, DataRetorno, DataSaida, "O veículo deve ser devolvido após a data de retirada");        
        VerificaSeguro(ref erros, SeguroCondutor, SeguroTerceiro);

        return erros;
    }

    protected void VerificaSeguro(ref List<string> erros, bool seguroCondutor, bool seguroTerceiro)
    {
        if (seguroCondutor && seguroTerceiro)
            erros.Add($"O seguro cobre cliente/condutor 'ou' terceiros");
    }

}