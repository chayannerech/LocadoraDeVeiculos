namespace LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
public class Configuracao()
{
    public decimal Gasolina { get; set; }
    public decimal Etanol { get; set; }
    public decimal Diesel { get; set; }
    public decimal GNV { get; set; }

    public List<string> Validar()
    {
        List<string> erros = [];

        VerificaNulo(ref erros, Gasolina, "Gasolina");
        VerificaNulo(ref erros, Etanol, "Etanol");
        VerificaNulo(ref erros, Diesel, "Diesel");
        VerificaNulo(ref erros, GNV, "GNV");

        return erros;
    }
}