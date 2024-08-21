using LocadoraDeVeiculos.Dominio.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.Compartilhado;

public abstract class RepositorioBaseEmOrm<TEntidade> where TEntidade : EntidadeBase
{
    protected readonly LocadoraDeVeiculosDbContext _dbContext;
    protected abstract DbSet<TEntidade> ObterRegistros();

    protected RepositorioBaseEmOrm(LocadoraDeVeiculosDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    #region CRUD
    public void Inserir(TEntidade entidade)
    {
        ObterRegistros().Add(entidade);

        _dbContext.SaveChanges();
    }

    public void Editar(TEntidade entidade)
    {
        ObterRegistros().Update(entidade);

        _dbContext.SaveChanges();
    }

    public void Excluir(TEntidade entidade)
    {
        ObterRegistros().Remove(entidade);

        _dbContext.SaveChanges();
    }
    #endregion

    public virtual TEntidade? SelecionarPorId(int id) 
        => ObterRegistros().FirstOrDefault(r => r.Id == id);
    public virtual List<TEntidade> SelecionarTodos() 
        => [.. ObterRegistros()];
}