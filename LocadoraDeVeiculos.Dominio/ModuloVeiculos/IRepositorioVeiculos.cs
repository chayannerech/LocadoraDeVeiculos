using LocadoraDeVeiculos.Dominio.Compartilhado;
namespace LocadoraDeVeiculos.Dominio.ModuloVeiculos;

public interface IRepositorioVeiculos : IRepositorio<Veiculos>
{
    List<IGrouping<string, Veiculos>> ObterVeiculosAgrupadosPorGrupo(int value);
    List<IGrouping<string, Veiculos>> ObterVeiculosAgrupadosPorGrupo();
}