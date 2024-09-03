using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloCondutor;

public class RepositorioCondutorEmOrm : RepositorioBaseEmOrm<Condutor>, IRepositorioCondutor
{
    public RepositorioCondutorEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Condutor> ObterRegistros() 
        => _dbContext.Condutores;

    public Condutor? SelecionarPorId(int id)
    {
        return _dbContext.Condutores
            .Include(c => c.Cliente)
            .FirstOrDefault(s => s.Id == id);
    }

    public List<Condutor> SelecionarTodos()
        => [.. _dbContext.Condutores
            .Include(c => c.Cliente)
            .AsNoTracking()];

    public List<Condutor> Filtrar(Func<Condutor, bool> predicate)
        => ObterRegistros()
            .Include(c => c.Cliente)
            .Where(predicate)
            .ToList();
}