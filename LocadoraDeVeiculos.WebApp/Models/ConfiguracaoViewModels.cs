using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirConfiguracaoViewModel
{
    [Required(ErrorMessage = "O preço da gasolina é obrigatório")]
    [Range(1, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Gasolina { get; set; }


    [Required(ErrorMessage = "O preço do diesel é obrigatório")]
    [Range(1, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Diesel { get; set; }


    [Required(ErrorMessage = "O preço do gás natural é obrigatório")]
    [Range(1, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal GNV { get; set; }


    [Required(ErrorMessage = "O preço do etanol é obrigatório")]
    [Range(1, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Etanol { get; set; }
}

public class EditarConfiguracaoViewModel : InserirConfiguracaoViewModel { }

public class DetalhesConfiguracaoViewModel
{
    public decimal Gasolina { get; set; }
    public decimal Etanol { get; set; }
    public decimal Diesel { get; set; }
    public decimal GNV { get; set; }
}