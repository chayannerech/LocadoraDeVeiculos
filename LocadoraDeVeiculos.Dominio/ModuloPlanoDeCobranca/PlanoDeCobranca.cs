using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
namespace LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;

public class PlanoDeCobranca() : EntidadeBase
{
    public string Nome { get; set; }
    public GrupoDeAutomoveis GrupoDeAutomoveis { get; set; }

    public PlanoDeCobranca(string nome, GrupoDeAutomoveis grupoDeAutomoveis) : this()
    {
        Nome = nome;
        GrupoDeAutomoveis = grupoDeAutomoveis;
    }
}

