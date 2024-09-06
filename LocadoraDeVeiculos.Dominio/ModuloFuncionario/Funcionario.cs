using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.Compartilhado.Extensions;
namespace LocadoraDeVeiculos.Dominio.ModuloFuncionario;
public class Funcionario() : EntidadeBase
{
    public string Nome { get; set; }
    public DateTime DataAdmissao { get; set; }
    public decimal Salario { get; set; }
    public string Login { get; set; }

    public Funcionario(string nome, DateTime dataAdmissao, decimal salario, string login) : this()
    {
        Nome = nome;
        DataAdmissao = dataAdmissao;
        Salario = salario;
        Login = login;
    }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Nome, "Nome");
        VerificaNulo(ref erros, Login, "Login");
        VerificaDataInferior(ref erros, DataAdmissao, "A data de admissão deve ser inferior ao dia de hoje");
        VerificaNulo(ref erros, Salario, "Salário");

        return erros;
    }

    public override string ToString() => Nome.ToTitleCase();
}