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
    public bool Seguro { get; set; }
    public string SeguroPara {  get; set; }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Condutor, "Condutor");
        VerificaNulo(ref erros, Cliente, "Cliente");
        VerificaNulo(ref erros, PlanoDeCobranca, "PlanoDeCobranca");
        VerificaNulo(ref erros, Veiculo, "Veiculo");
        VerificaDataInferior(ref erros, DataSaida, "O veículo deve ser retirado hoje ou após o dia de hoje");
        VerificaDataInferior(ref erros, DataRetorno, DataSaida, "O veículo deve ser devolvido após a data de retirada");
        
        if (Seguro) VerificaNulo(ref erros, SeguroPara, "SeguroPara");

        return erros;
    }
}