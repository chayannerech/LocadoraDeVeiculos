using LocadoraDeVeiculos.Dominio.Compartilhado;
namespace LocadoraDeVeiculos.Dominio.ModuloFuncionario;
public class Funcionario() : EntidadeBase
{
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }    
}