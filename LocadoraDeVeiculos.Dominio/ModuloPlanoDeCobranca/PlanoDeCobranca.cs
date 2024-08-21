using LocadoraDeVeiculos.Dominio.Compartilhado;
namespace LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;

public class PlanoDeCobranca() : EntidadeBase
{
    public string Nome { get; set; }

    public PlanoDeCobranca(string nome) : this()
    {
        Nome = nome;
    }
}

