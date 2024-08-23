using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirPlanoDeCobrancaViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(6, ErrorMessage = "O nome deve conter ao menos 6 caracteres")]
    public string Nome { get; set; }
}

public class EditarPlanoDeCobrancaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(6, ErrorMessage = "O nome deve conter ao menos 6 caracteres")]
    public string Nome { get; set; }
}

public class ListarPlanoDeCobrancaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
}

public class DetalhesPlanoDeCobrancaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<PlanoDeCobranca> Planos { get; set; }
}