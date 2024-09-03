using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
public class GrupoDeAutomoveis() : EntidadeBase
{
    public string Nome { get; set; }
    public decimal PrecoDiaria { get; set; }
    public decimal PrecoDiariaControlada {  get; set; }
    public decimal PrecoLivre { get; set; }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Nome, "Nome");
        return erros;
    }

    public override string ToString() => Nome.ToTitleCase();
}