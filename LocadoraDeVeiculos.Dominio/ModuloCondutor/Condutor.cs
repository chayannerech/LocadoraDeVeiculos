using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
namespace LocadoraDeVeiculos.Dominio.ModuloCondutor;
public class Condutor() : EntidadeBase
{
    public Cliente Cliente { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string CPF { get; set; }
    public DateTime ValidadeCNH { get; set; }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Cliente, "Cliente");
        VerificaNulo(ref erros, Nome, "Nome");
        VerificaNulo(ref erros, Email, "Email");
        VerificaNulo(ref erros, Telefone, "Telefone");
        VerificaNulo(ref erros, CPF, "CPF");
        VerificaDataInferior(ref erros, ValidadeCNH, "A CNH não pode estar vencida");

        return erros;
    }

    public override string ToString() => Nome.ToTitleCase();
}