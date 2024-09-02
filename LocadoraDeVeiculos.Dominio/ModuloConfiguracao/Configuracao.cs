using LocadoraDeVeiculos.Dominio.ModuloUsuario;
namespace LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
public class Configuracao()
{
    public int Id { get; set; }
    public decimal Gasolina { get; set; }
    public decimal Etanol { get; set; }
    public decimal Diesel { get; set; }
    public decimal GNV { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }


    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Gasolina, "Gasolina");
        VerificaNulo(ref erros, Etanol, "Etanol");
        VerificaNulo(ref erros, Diesel, "Diesel");
        VerificaNulo(ref erros, GNV, "GNV");

        return erros;
    }

    protected void VerificaNulo(ref List<string> erros, decimal campoTestado, string mostraCampo)
    {
        if (campoTestado == 0)
            erros.Add($"\nO campo \"{mostraCampo}\" é obrigatório. Tente novamente ");
    }
}