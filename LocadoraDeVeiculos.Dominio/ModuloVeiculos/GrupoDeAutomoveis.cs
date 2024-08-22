using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
namespace LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;

public class GrupoDeAutomoveis : EntidadeBase
{
    public string Nome { get; set; }
    public List<PlanoDeCobranca> Planos { get; set; }

    public GrupoDeAutomoveis() { }
    public GrupoDeAutomoveis(string nome)
    {
        Nome = nome.ToTitleCase();
        Planos = [];
    }
    public override string ToString() => Nome;
}

