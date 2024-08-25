using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
namespace LocadoraDeVeiculos.Dominio.ModuloConfiguracaoe;
public interface IRepositorioConfiguracao 
{
    void Inserir(Configuracao entidade);
    void Editar(Configuracao entidadeAtualizada);
}