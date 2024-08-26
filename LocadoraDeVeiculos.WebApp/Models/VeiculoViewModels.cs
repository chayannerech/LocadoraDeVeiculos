using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirVeiculosViewModel
{
    [Required(ErrorMessage = "O grupo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O grupo é obrigatório")]
    public int GrupoId { get; set; }


    [Required(ErrorMessage = "A placa é obrigatória")]
    [RegularExpression(@"^[A-Za-z0-9]{7}$", ErrorMessage = "A placa deve conter exatamente 7 caracteres alfanuméricos")]
    public string Placa { get; set; }


    [Required(ErrorMessage = "A marca é obrigatória")]
    [MinLength(3, ErrorMessage = "A marca deve conter pelo menos 3 caracteres")]
    public string Marca { get; set; }


    [Required(ErrorMessage = "A cor é obrigatória")]
    [MinLength(4, ErrorMessage = "A cor deve conter pelo menos 4 caracteres")]
    public string Cor { get; set; }


    [Required(ErrorMessage = "O modelo é obrigatório")]
    [MinLength(4, ErrorMessage = "O modelo deve conter pelo menos 4 caracteres")]
    public string Modelo { get; set; }


    [Required(ErrorMessage = "O tipo de combustível é obrigatório")]
    public string TipoCombustivel { get; set; }


    [Required(ErrorMessage = "A capacidade de combustível é obrigatória")]
    [Range(30, 120, ErrorMessage = "A capacidade deve estar entre 30 e 120L")]
    public int CapacidadeCombustivel { get; set; }


    [Required(ErrorMessage = "O ano é obrigatório")]
    [Range(1950, 2024, ErrorMessage = "O ano deve estar entre 1950 e 2024")]
    public int Ano { get; set; }


    public IEnumerable<SelectListItem>? Grupos { get; set; }
    public IEnumerable<SelectListItem>? TiposDeCombustiveis { get; set; }


    [Required(ErrorMessage = "A foto do veículo é obrigatória")]
    public IFormFile Foto { get; set; }
    public byte[] ImagemEmBytes
    {
        get => Foto != null ? ConverterImagemParaArrayDeBytes(Foto) : null!;
        set { }
    }
    public string TipoDaImagem
    {
        get => Foto != null ? Foto.ContentType : null!;
        set { }
    }
    private byte[] ConverterImagemParaArrayDeBytes(IFormFile imagem)
    {
        if (imagem == null || imagem.Length == 0)
        {
            throw new ArgumentNullException(nameof(imagem), "A imagem não pode ser nula ou vazia");
        }

        using var memoryStream = new MemoryStream();
        imagem.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}

public class EditarVeiculosViewModel : InserirVeiculosViewModel
{    
    public int Id { get; set; }
    public byte[] ImagemEmBytes { get; set; }
    public string TipoDaImagem { get; set; }
}

public class ListarVeiculosViewModel
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public string Marca { get; set; }
    public string Cor { get; set; }
    public string Modelo { get; set; }
    public string TipoCombustivel { get; set; }
    public DateTime Ano { get; set; }
}

public class AgrupamentoVeiculosPorGrupoViewModel
{
    public string Grupo { get; set; }
    public IEnumerable<ListarVeiculosViewModel> Veiculos { get; set; }
}

public class DetalhesVeiculosViewModel
{
    public int Id { get; set; }
    public string GrupoNome { get; set; }
    public string Placa { get; set; }
    public string Marca { get; set; }
    public string Cor { get; set; }
    public string Modelo { get; set; }
    public string TipoCombustivel { get; set; }
    public int CapacidadeCombustivel { get; set; }
    public int Ano { get; set; }
    public byte[] ImagemEmBytes { get; set; }
    public string TipoDaImagem { get; set; }
}