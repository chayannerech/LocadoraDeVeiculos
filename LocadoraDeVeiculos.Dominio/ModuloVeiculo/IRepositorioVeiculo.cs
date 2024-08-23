using LocadoraDeVeiculos.Dominio.Compartilhado;
namespace LocadoraDeVeiculos.Dominio.ModuloVeiculos;

public interface IRepositorioVeiculo : IRepositorio<Veiculo>
{
    List<IGrouping<string, Veiculo>> ObterVeiculosAgrupadosPorGrupo(int value);
    List<IGrouping<string, Veiculo>> ObterVeiculosAgrupadosPorGrupo();
}