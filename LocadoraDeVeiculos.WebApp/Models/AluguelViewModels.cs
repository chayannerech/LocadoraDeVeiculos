using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class ListarAluguelViewModel
{
    public int Id { get; set; }
    public CategoriaDePlanoEnum CategoriaPlano { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public DateTime DataRetornoReal { get; set; }
    public decimal ValorTotal { get; set; }
    public bool Ativo { get; set; }

    public string? CondutorNome { get; set; }
    public string? VeiculoPlaca { get; set; }
}

public class InserirAluguelViewModel
{
    [Required(ErrorMessage = "O condutor é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O condutor é obrigatório")]
    public int CondutorId { get; set; }


    [Required(ErrorMessage = "O cliente é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O cliente é obrigatório")]
    public int ClienteId { get; set; }


    [Required(ErrorMessage = "O grupo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O grupo é obrigatório")]
    public int GrupoId { get; set; }


    [Required(ErrorMessage = "O veículo é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O veículo é obrigatório")]
    public int VeiculoId { get; set; }


    [Required(ErrorMessage = "O plano é obrigatório")]
    public CategoriaDePlanoEnum CategoriaPlano { get; set; }


    [Required(ErrorMessage = "A data de retirada é obrigatória")]
    [DataType(DataType.Date)]
    [DataMenorQueHoje(ErrorMessage = "O veículo deve ser retirado em uma data superior à de hoje")]
    public DateTime DataSaida { get; set; }


    [Required(ErrorMessage = "A data de devolução é obrigatória")]
    [DataType(DataType.Date)]
    [DataMenorQue(ErrorMessage = "A data de retorno deve ser superior à data de saída")]
    public DateTime DataRetornoPrevista { get; set; }


    public decimal ValorTotal { get; set; }
    public string? TaxasSelecionadasId { get; set; }
    public IEnumerable<Condutor>? Condutores { get; set; }
    public IEnumerable<SelectListItem>? Clientes { get; set; }
    public IEnumerable<GrupoDeAutomoveis>? Grupos { get; set; }
    public IEnumerable<Veiculo>? Veiculos { get; set; }
    public IEnumerable<SelectListItem>? Categorias { get; set; }
    public IEnumerable<Taxa>? Taxas { get; set; }
    public IEnumerable<Taxa>? Seguros { get; set; }
}

public class EditarAluguelViewModel : InserirAluguelViewModel
{
    public int Id { get; set; }
}

public class DetalhesAluguelViewModel
{
    public int Id { get; set; }
    public string? CondutorNome { get; set; }
    public string? ClienteNome { get; set; }
    public string? GrupoNome { get; set; }
    public int VeiculoId { get; set; }
    public string? VeiculoPlaca { get; set; }
    public byte[]? ImagemEmBytes { get; set; }
    public string? TipoDaImagem { get; set; }
    public CategoriaDePlanoEnum CategoriaPlano { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public DateTime DataRetornoReal { get; set; }
    public bool Ativo { get; set; }
    public decimal ValorTotal { get; set; }
}

public class DevolverAluguelViewModel
{
    public int Id { get; set; }
    public Condutor? Condutor { get; set; }
    public Cliente? Cliente { get; set; }
    public PlanoDeCobranca? PlanoDeCobranca { get; set; }
    public GrupoDeAutomoveis? Grupo { get; set; }
    public Veiculo? Veiculo { get; set; }
    public byte[]? ImagemEmBytes { get; set; }
    public string? TipoDaImagem { get; set; }
    public CategoriaDePlanoEnum CategoriaPlano { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public string? TaxasSelecionadasId { get; set; }
    public Configuracao? Configuracao { get; set; }


    [Required(ErrorMessage = "A quilometragem inicial é obrigatória")]
    [Range(1, 50000, ErrorMessage = "O valor deve ser maior que zero")]
    public int? KmInicial {  get; set; }


    [Required(ErrorMessage = "A quilometragem atual é obrigatória")]
    [Range(1, 50000, ErrorMessage = "O valor deve ser maior que zero")]
    [KmMaior(ErrorMessage = "O km atual deve ser superior ao km inicial")]
    public int? KmAtual {  get; set; }


    [Required(ErrorMessage = "A data de devolução é obrigatória")]
    [DataType(DataType.Date)]
    [DataDevolucaoMenorQue(ErrorMessage = "A data de retorno deve ser superior à data de saída")]
    public DateTime DataRetornoReal { get; set; }

    public int DiasPrevistos { get => (DataRetornoPrevista - DataSaida).Days; set { } }

    public decimal ValorTotal { get; set; }
    public IEnumerable<Taxa>? Taxas { get; set; }
}

public class DataMenorQueAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var model = (InserirAluguelViewModel)validationContext.ObjectInstance;

        if (model.DataRetornoPrevista <= model.DataSaida)
            return new ValidationResult("");

        return ValidationResult.Success;
    }
}

public class DataDevolucaoMenorQueAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var model = (DevolverAluguelViewModel)validationContext.ObjectInstance;

        if (model.DataRetornoReal <= model.DataSaida)
            return new ValidationResult("");

        return ValidationResult.Success;
    }
}

public class KmMaiorAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var model = (DevolverAluguelViewModel)validationContext.ObjectInstance;

        if (model.KmAtual <= model.KmInicial)
            return new ValidationResult("O km atual deve ser superior ao km inicial");

        return ValidationResult.Success;
    }
}