namespace LocadoraDeVeiculos.Dominio.Compartilhado;
public abstract class EntidadeBase
{
    public int Id { get; set; }
    /*    public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }*/

    protected void VerificaNulo(ref List<string> erros, string campoTestado, string mostraCampo)
    {
        if (string.IsNullOrEmpty(campoTestado))
            erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaNulo(ref List<string> erros, int campoTestado, string mostraCampo)
    {
        if (string.IsNullOrEmpty(campoTestado.ToString()))
            erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaNulo(ref List<string> erros, EntidadeBase campoTestado, string mostraCampo)
    {
        if (campoTestado is null)
            erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
    protected void VerificaNulo(ref List<string> erros, Enum campoTestado, string mostraCampo)
    {
        if (campoTestado is null)
            erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
}