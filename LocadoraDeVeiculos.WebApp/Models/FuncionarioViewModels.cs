﻿using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirFuncionarioViewModel : EditarFuncionarioViewModel
{
    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve conter ao menos 6 caracteres")]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }


    [Display(Name = "Confirme a senha")]
    [DataType(DataType.Password)]
    [Compare("Senha", ErrorMessage = "As senhas não conferem")]
    public string? ConfirmarSenha { get; set; }
}

public class EditarFuncionarioViewModel
{
    public int Id { get; set; }


    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(4, ErrorMessage = "O nome deve conter ao menos 4 caracteres")]
    public string? Nome { get; set; }


    [Required(ErrorMessage = "O login é obrigatório")]
    [MinLength(4, ErrorMessage = "O login deve conter ao menos 4 caracteres")]
    [LoginValido(ErrorMessage = "O Login não deve conter espaços")]
    public string? Login { get; set; }


    [Required(ErrorMessage = "A data de admissão é obrigatória")]
    [DataMenorQueHoje(ErrorMessage = "A data de admissão deve ser inferior ao dia de hoje")]
    public DateTime DataAdmissao { get; set; }


    [Required(ErrorMessage = "O salário é obrigatório")]
    [Range(1, 100000, ErrorMessage = "O salário deve ser maior que zero")]
    public decimal Salario { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress]
    public string? Email { get; set; }
}

public class ListarFuncionarioViewModel
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Login { get; set; }
}

public class DetalhesFuncionarioViewModel
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public DateTime DataAdmissao { get; set; }
    public decimal Salario { get; set; }
}

public class LoginValidoAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string data)
        {
            if (!data.ToCharArray().Grupoins(' '))
                return ValidationResult.Success!;
            else
                return new ValidationResult(ErrorMessage ?? "O Login não deve conter espaços");
        }

        return new ValidationResult("Login inválido");
    }
}