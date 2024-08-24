using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.WebApp.Models;

public class InserirGrupoDeAutomoveisViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(6, ErrorMessage = "O nome deve conter ao menos 6 caracteres")]
    public string Nome { get; set; }
}

public class EditarGrupoDeAutomoveisViewModel : InserirGrupoDeAutomoveisViewModel
{
    public int Id { get; set; }
}

public class ListarGrupoDeAutomoveisViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
}

public class DetalhesGrupoDeAutomoveisViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<PlanoDeCobranca> Planos { get; set; }
}