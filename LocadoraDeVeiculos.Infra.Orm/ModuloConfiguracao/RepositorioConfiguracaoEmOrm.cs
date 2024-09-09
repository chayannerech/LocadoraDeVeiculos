using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracaoe;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloConfiguracao;
public class RepositorioConfiguracaoEmOrm(LocadoraDeVeiculosDbContext DbContext) : IRepositorioConfiguracao
{
    public void Inserir(Configuracao entidade)
    {
        ObterRegistros().Add(entidade);

        DbContext.SaveChanges();
    }
    public void Editar(Configuracao entidade)
    {
        ObterRegistros().Update(entidade);

        DbContext.SaveChanges();
    }

    protected DbSet<Configuracao> ObterRegistros() 
        => DbContext.Configuracoes;

    public Configuracao? Selecionar(int usuarioId)
        => ObterRegistros().Where(c => c.UsuarioId == usuarioId).ToList().Count == 0 ? 
            null : ObterRegistros().Where(c => c.UsuarioId == usuarioId).ToList().First();
}