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

    public PlanoDeCobranca(GrupoDeAutomoveis grupoDeAutomoveis, decimal precoDiaria, decimal precoKm, int kmDisponivel, decimal precoDiariaControlada, decimal precoExtrapolado, decimal precoLivre) : this()
    {
        GrupoDeAutomoveis = grupoDeAutomoveis;
        PrecoDiaria = precoDiaria;
        PrecoKm = precoKm;
        KmDisponivel = kmDisponivel;
        PrecoDiariaControlada = precoDiariaControlada;
        PrecoExtrapolado = precoExtrapolado;
        PrecoLivre = precoLivre;
    }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, GrupoDeAutomoveis, "Grupo de Automóveis");
        VerificaNulo(ref erros, PrecoDiaria, "Preço diário");
        VerificaNulo(ref erros, PrecoKm, "Preço por Km");
        VerificaNulo(ref erros, KmDisponivel, "Quilometragem disponível");
        VerificaNulo(ref erros, PrecoDiariaControlada, "Preço da diária controlada");
        VerificaNulo(ref erros, PrecoExtrapolado, "Preço por Km extrapolado");
        VerificaNulo(ref erros, PrecoLivre, "Preço livre");

        return erros;
    }
    public override string ToString() => $"Plano para o Grupo {GrupoDeAutomoveis}";
}