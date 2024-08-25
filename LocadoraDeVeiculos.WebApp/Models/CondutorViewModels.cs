using LocadoraDeVeiculos.Dominio.ModuloCliente;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirCondutorViewModel
{
    [Required(ErrorMessage = "O cliente é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O cliente é obrigatório")]
    public int ClienteId { get; set; }

    public List<Cliente>? Clientes { get; set; }


    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(6, ErrorMessage = "O nome deve conter ao menos 6 caracteres")]
    public string Nome { get; set; }


    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Insira um email válido")]
    public string Email { get; set; }


    [Required(ErrorMessage = "O telefone é obrigatório")]
    public string Telefone { get; set; }


    [Required(ErrorMessage = "O CPF é obrigatório")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$",
        ErrorMessage = "O CPF deve estar em formato válido")]
    public string CPF { get; set; }


    [Required(ErrorMessage = "A CNH é obrigatória")]
    [Length(11, 11, ErrorMessage = "A CNH deve estar em formato válido")]
    public string CNH { get; set; }


    [Required(ErrorMessage = "A data é obrigatória")]
    [DataMenorQueHoje(ErrorMessage = "A CNH está vencida")]
    public DateTime ValidadeCNH { get; set; }

    public bool Check { get; set; }
}

public class EditarCondutorViewModel : InserirCondutorViewModel
{
    public int Id { get; set; }
}

public class ListarCondutorViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public Cliente Cliente { get; set; }
    public string NomeCliente { get => Cliente.Nome; set { } }
    public string CPF { get; set; }
    public string CNH { get; set; }
    public DateTime ValidadeCNH { get; set; }
}

public class DetalhesCondutorViewModel
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public string NomeCliente { get => Cliente.Nome; set { } }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string CPF { get; set; }
    public string CNH { get; set; }
    public DateTime ValidadeCNH { get; set; }
}

public class DataMenorQueHojeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime data)
        {
            if (data > DateTime.Today)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage ?? "A data deve ser menor que a data atual");
        }

        return new ValidationResult("Data inválida.");
    }
}