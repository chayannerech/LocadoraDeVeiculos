using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
namespace LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
public class GrupoDeAutomoveis() : EntidadeBase
{
    public string Nome { get; set; }
    public List<PlanoDeCobranca> Planos { get; set; } = [];

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Nome, "Nome");
        return erros;
    }

    public override string ToString() => Nome;
}

