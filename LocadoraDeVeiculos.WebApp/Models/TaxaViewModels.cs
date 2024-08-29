using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirTaxaViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(4, ErrorMessage = "O nome deve conter ao menos 4 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O preço da taxa ou serviço é obrigatório")]
    [Range(1, 10000, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O modo de cobrança é obrigatório")]
    public bool PrecoFixo { get; set; }

    public bool Seguro { get; set; }
}

public class EditarTaxaViewModel : InserirTaxaViewModel
{
    public int Id { get; set; }
}

public class ListarTaxaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public bool PrecoFixo { get; set; }
}

public class DetalhesTaxaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public bool PrecoFixo { get; set; }
}