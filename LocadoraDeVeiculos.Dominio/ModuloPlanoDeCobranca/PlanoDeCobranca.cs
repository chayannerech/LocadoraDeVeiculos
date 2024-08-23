using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
namespace LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;

public class PlanoDeCobranca() : EntidadeBase
{
    public GrupoDeAutomoveis GrupoDeAutomoveis { get; set; }
    //public Enum Categoria { get; set; }
    public decimal PrecoDiaria { get; set; }
    public decimal PrecoKm { get; set;}
    public int KmDisponivel { get; set; }
    public decimal PrecoDiariaControlada { get; set; }
    public decimal PrecoExtrapolado { get; set; }
    public decimal PrecoLivre { get; set; }
}

