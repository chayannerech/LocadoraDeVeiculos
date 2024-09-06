using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloCondutor;
public class RepositorioCondutorEmOrm : RepositorioBaseEmOrm<Condutor>, IRepositorioCondutor
{
    public RepositorioCondutorEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Condutor> ObterRegistros()
        => _dbContext.Condutores;

    public override Condutor? SelecionarPorId(int id)
        => ObterRegistros()
            .Where(c => c.Ativo)
            .Include(c => c.Cliente)
            .FirstOrDefault(c => c.Id == id);

    public override List<Condutor> SelecionarTodos()
        => [.. ObterRegistros()
            .Where(c => c.Ativo)
            .Include(c => c.Cliente)
            .AsNoTracking()];

    public override List<Condutor> Filtrar(Func<Condutor, bool> predicate)
        => ObterRegistros()
            .Include(e => e.Cliente)
            .Where(e => e.Ativo)
            .Where(predicate)
            .ToList();
}