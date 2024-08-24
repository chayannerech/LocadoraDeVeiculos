using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
namespace LocadoraDeVeiculos.Dominio.ModuloCliente;
public class Cliente() : EntidadeBase
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public bool PessoaFisica { get; set; }
    public string Documento { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public int Numero { get; set; }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Nome, "Nome");
        VerificaNulo(ref erros, Email, "Email");
        VerificaNulo(ref erros, Telefone, "Telefone");
        VerificaNulo(ref erros, Documento, "Documento");
        VerificaNulo(ref erros, Estado, "Estado");
        VerificaNulo(ref erros, Cidade, "Cidade");
        VerificaNulo(ref erros, Bairro, "Bairro");
        VerificaNulo(ref erros, Rua, "Rua");
        VerificaNulo(ref erros, Numero, "Numero");

        return erros;
    }

    public override string ToString() => Nome.ToTitleCase();
}