using LocadoraDeVeiculos.Dominio.ModuloCliente;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirClienteViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(6, ErrorMessage = "O nome deve conter ao menos 6 caracteres")]
    public string Nome { get; set; }


    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Insira um email válido")]
    public string Email { get; set; }


    [Required(ErrorMessage = "O telefone é obrigatório")]
    public string Telefone { get; set; }


    [Required(ErrorMessage = "Informe o tipo de cliente")]
    public bool? PessoaFisica { get; set; }


    [Required(ErrorMessage = "O documento é obrigatório.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$|^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$",
        ErrorMessage = "O documento deve estar em formato válido")]
    public string Documento { get; set; }


    [Required(ErrorMessage = "O estado é obrigatório")]
    public string Estado { get; set; }


    [Required(ErrorMessage = "A cidade é obrigatória")]
    [MinLength(4, ErrorMessage = "A cidade deve conter ao menos 4 caracteres")]
    public string Cidade { get; set; }


    [Required(ErrorMessage = "O bairro é obrigatório")]
    [MinLength(4, ErrorMessage = "O bairro deve conter ao menos 4 caracteres")]
    public string Bairro { get; set; }


    [Required(ErrorMessage = "A rua é obrigatória")]
    [MinLength(6, ErrorMessage = "A rua deve conter ao menos 4 caracteres")]
    public string Rua { get; set; }


    [Required(ErrorMessage = "O número é obrigatório")]
    [Range(1, 50000, ErrorMessage = "O número deve ser maior que zero")]
    public int Numero { get; set; }
}

public class EditarClienteViewModel : InserirClienteViewModel
{
    public int Id { get; set; }
}

public class ListarClienteViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Documento { get; set; }
    public bool PessoaFisica { get; set; }
}

public class DetalhesClienteViewModel
{
    public int Id { get; set; }
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
}