using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
namespace LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
public class PlanoDeCobranca() : EntidadeBase
{
    public GrupoDeAutomoveis GrupoDeAutomoveis { get; set; }
    public decimal PrecoDiaria { get; set; } = 0;
    public decimal PrecoKm { get; set; } = 0;
    public int KmDisponivel { get; set; } = 0;
    public decimal PrecoDiariaControlada { get; set; } = 0;
    public decimal PrecoExtrapolado { get; set; } = 0;
    public decimal PrecoLivre { get; set; } = 0;

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Categoria, "Categoria");
        VerificaNulo(ref erros, GrupoDeAutomoveis, "Grupo de Automóveis");

        return erros;
    }
    public override string ToString() => $"Plano para o Grupo {GrupoDeAutomoveis}";
}