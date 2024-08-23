using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class PropriedadesVeiculosViewModel
{
    [Required(ErrorMessage = "O grupo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O grupo é obrigatório")]
    public int GrupoId { get; set; }


    [Required(ErrorMessage = "A placa é obrigatória")]
    [Length(7, 7, ErrorMessage = "A placa deve conter 7 caracteres")]
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
    [Range(50, 10000, ErrorMessage = "A capacidade deve estar entre 50l e 10000l")]
    public int CapacidadeCombustivel { get; set; }


    [Required(ErrorMessage = "O ano é obrigatório")]
    [Range(1950, 2024, ErrorMessage = "O ano deve estar entre 1950 e 2024")]
    public int Ano { get; set; }

    public IEnumerable<SelectListItem>? Grupos { get; set; }

}

public class InserirVeiculosViewModel : PropriedadesVeiculosViewModel
{
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

public class EditarVeiculosViewModel : PropriedadesVeiculosViewModel
{
    public int Id { get; set; }

    public IFormFile Foto
    {
        get => CriarFormFile(ImagemEmBytes, TipoDaImagem);
        set {}
    }
    public byte[] ImagemEmBytes { get; set; }
    public string TipoDaImagem { get; set; }

    private IFormFile CriarFormFile(byte[] imagemEmBytes, string tipoDaImagem)
    {
        if (imagemEmBytes == null || imagemEmBytes.Length == 0)
        {
            return null;
        }

        var stream = new MemoryStream(imagemEmBytes);
        return new FormFile(stream, 0, imagemEmBytes.Length, null, $"arquivo.{ObterExtensao(tipoDaImagem)}")
        {
            Headers = new HeaderDictionary(),
            ContentType = tipoDaImagem
        };
    }
    private string ObterExtensao(string tipoDaImagem)
    {
        return tipoDaImagem switch
        {
            "image/jpeg" => "jpg",
            "image/png" => "png",
            "image/gif" => "gif",
            "image/bmp" => "bmp",
            _ => "bin"
        };
    }
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
    public string Nome { get; set; }
    public List<PlanoDeCobranca> Planos { get; set; }
}