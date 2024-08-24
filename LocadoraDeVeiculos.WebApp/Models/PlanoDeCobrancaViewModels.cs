using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class ListarPlanoDeCobrancaViewModel
{
    public int Id { get; set; }
    public CategoriaDePlanoEnum Categoria { get; set; }
    public decimal PrecoDiaria { get; set; } = 0;
    public decimal PrecoKm { get; set; } = 0;
    public int KmDisponivel { get; set; } = 0;
    public decimal PrecoDiariaControlada { get; set; } = 0;
    public decimal PrecoExtrapolado { get; set; } = 0;
    public decimal PrecoLivre { get; set; } = 0;
}

public class InserirPlanoDeCobrancaViewModel
{
    [Required(ErrorMessage = "O grupo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O grupo é obrigatório")]
    public int GrupoId { get; set; }

    [Required(ErrorMessage = "O preço da diária é obrigatório")]
    [Range(0, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal PrecoDiaria { get; set; } = 0;

    [Required(ErrorMessage = "O preço por quilômetro é obrigatório")]
    [Range(0, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal PrecoKm { get; set; } = 0;

    [Required(ErrorMessage = "A quilometragem disponível é obrigatória")]
    [Range(0, 10000, ErrorMessage = "A quilometragem deve ser maior que zero")]
    public int KmDisponivel { get; set; } = 0;

    [Required(ErrorMessage = "O preço da diária controlada é obrigatório")]
    [Range(0, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal PrecoDiariaControlada { get; set; } = 0;

    [Required(ErrorMessage = "O preço por quilômetro extrapolado é obrigatório")]
    [Range(0, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal PrecoExtrapolado { get; set; } = 0;

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Range(0, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal PrecoLivre { get; set; } = 0;

    public IEnumerable<SelectListItem>? Grupos { get; set; }
}

public class EditarPlanoDeCobrancaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(6, ErrorMessage = "O nome deve conter ao menos 6 caracteres")]
    public string Nome { get; set; }

    public IEnumerable<SelectListItem>? Grupos { get; set; }
}

public class DetalhesPlanoDeCobrancaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<PlanoDeCobranca> Planos { get; set; }
}