using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
namespace LocadoraDeVeiculos.Dominio.ModuloTaxa;
public class Taxa() : EntidadeBase
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public bool PrecoFixo { get; set; }
    public bool Seguro { get; set; }

    public Taxa(string nome, decimal preco) : this()
    {
        Nome = nome;
        Preco = preco;
    }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Nome, "Nome");
        VerificaNulo(ref erros, Preco, "Preço");

        return erros;
    }

    public override string ToString() => Nome.ToTitleCase();
}