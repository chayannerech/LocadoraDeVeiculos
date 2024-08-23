using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
namespace LocadoraDeVeiculos.Dominio.ModuloVeiculos;
public class Veiculo() : EntidadeBase
{
    public string Placa { get; set; }
    public string Marca { get; set; }
    public string Cor { get; set; }
    public string Modelo { get; set; }
    public string TipoCombustivel { get; set; }
    public int CapacidadeCombustivel { get; set; }
    public int Ano { get; set; }
    public byte[] ImagemEmBytes { get; set; }
    public string TipoDaImagem { get; set; }
    public GrupoDeAutomoveis GrupoDeAutomoveis { get; set; }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Placa, "Placa");
        VerificaNulo(ref erros, Marca, "Marca");
        VerificaNulo(ref erros, Cor, "Cor");
        VerificaNulo(ref erros, Modelo, "Modelo");
        VerificaNulo(ref erros, TipoCombustivel, "TipoCombustivel");
        VerificaNulo(ref erros, CapacidadeCombustivel, "CapacidadeCombustivel");
        VerificaNulo(ref erros, Ano, "Ano");
        VerificaNulo(ref erros, ImagemEmBytes, "Foto");
        VerificaNulo(ref erros, GrupoDeAutomoveis, "Grupo de Automóveis");
        VerificaDataFutura(ref erros, Ano);

        return erros;
    }

    public override string ToString() => $"Veículo placa: {Placa}";
    protected void VerificaNulo(ref List<string> erros, byte[] campoTestado, string mostraCampo)
    {
        if (campoTestado is null)
            erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaDataFutura(ref List<string> erros, int campoTestado)
    {
        if (campoTestado > DateTime.Now.Year)
            erros.Add($"\nO ano precisa ser inferior a data atual. Tente novamente ");
    }
}