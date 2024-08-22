using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
namespace LocadoraDeVeiculos.Dominio.ModuloVeiculos;

public class Veiculos : EntidadeBase
{
    public string Nome { get; set; }
    public List<PlanoDeCobranca> Planos { get; set; }

    public Veiculos() { }
    public Veiculos(string nome)
    {
        Nome = nome.ToTitleCase();
        Planos = [];
    }
    public override string ToString() => Nome;
}

