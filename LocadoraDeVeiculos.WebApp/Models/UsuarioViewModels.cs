using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;
public class RegistrarViewModel
{
    [Required]
    public string? Usuario { get; set; }


    [Required]
    [EmailAddress]
    public string? Email { get; set; }


    [Required]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }


    [Display(Name = "Confirme a senha")]
    [DataType(DataType.Password)]
    [Compare("Senha", ErrorMessage = "As senhas não conferem")]
    public string? ConfirmarSenha { get; set; }
}

public class LoginViewModel
{
    [Required]
    public string? Usuario { get; set; }


    [Required(ErrorMessage = "A senha é obrigatória")]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }


    [Display(Name = "Lembrar-me")]
    public bool LembrarMe { get; set; }
}