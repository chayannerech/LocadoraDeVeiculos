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

        if (Ano > DateTime.Now.Year)
            erros.Add("O ano não pode ser superior ao ano atual");

        return erros;
    }

    public override string ToString() => $"Placa: {Placa}";
}