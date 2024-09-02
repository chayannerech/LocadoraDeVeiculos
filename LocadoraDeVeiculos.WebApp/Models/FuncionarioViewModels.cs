using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirFuncionarioViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(4, ErrorMessage = "O nome deve conter ao menos 4 caracteres")]
    public string? Nome { get; set; }

    
    [Required(ErrorMessage = "O login é obrigatório")]
    [MinLength(6, ErrorMessage = "O login deve conter ao menos 6 caracteres")]
    public string? Login { get; set; }

    
    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve conter ao menos 6 caracteres")]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }
}

public class EditarFuncionarioViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(4, ErrorMessage = "O nome deve conter ao menos 4 caracteres")]
    public string? Nome { get; set; }
}

public class ListarFuncionarioViewModel
{
    public int Id { get; set; }
    public string? Nome { get; set; }
}

public class DetalhesFuncionarioViewModel
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Login { get; set; }
}