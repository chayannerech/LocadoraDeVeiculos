using LocadoraDeVeiculos.Dominio.ModuloUsuario;
namespace LocadoraDeVeiculos.Dominio.Compartilhado;
public abstract class EntidadeBase
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }


    protected void VerificaNulo(ref List<string> erros, string campoTestado, string mostraCampo)
    {
        if (string.IsNullOrEmpty(campoTestado))
            erros.Add($"O campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaNulo(ref List<string> erros, int campoTestado, string mostraCampo)
    {
        if (campoTestado == 0)
            erros.Add($"O campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaNulo(ref List<string> erros, decimal campoTestado, string mostraCampo)
    {
        if (campoTestado == 0)
            erros.Add($"O campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaNulo(ref List<string> erros, EntidadeBase campoTestado, string mostraCampo)
    {
        if (campoTestado is null)
            erros.Add($"O campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaDataInferior(ref List<string> erros, DateTime campoTestado, string mostraCampo)
    {
        if (campoTestado.AddDays(1) < DateTime.Now)
            erros.Add($"{mostraCampo}");
    }
    protected void VerificaDataInferior(ref List<string> erros, DateTime campoTestado, DateTime campoTestado2, string mostraCampo)
    {
        if (campoTestado < campoTestado2)
            erros.Add($"{mostraCampo}");
    }
}