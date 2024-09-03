using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
namespace LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
public class GrupoDeAutomoveis() : EntidadeBase
{
    public string Nome { get; set; }
    public decimal PrecoDiaria { get; set; }
    public decimal PrecoDiariaControlada {  get; set; }
    public decimal PrecoLivre { get; set; }

    public GrupoDeAutomoveis(string nome, decimal precoDiaria, decimal precoDiariaControlada, decimal precoLivre) : this()
    {
        Nome = nome;
        PrecoDiaria = precoDiaria;
        PrecoDiariaControlada = precoDiariaControlada;
        PrecoLivre = precoLivre;
    }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Nome, "Nome");
        return erros;
    }

    public override string ToString() => Nome.ToTitleCase();
}