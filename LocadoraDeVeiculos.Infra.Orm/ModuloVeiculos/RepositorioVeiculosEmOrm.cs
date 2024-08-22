using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;

public class RepositorioVeiculosEmOrm : RepositorioBaseEmOrm<Veiculos>, IRepositorioVeiculos
{
    public RepositorioVeiculosEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Veiculos> ObterRegistros() 
        => _dbContext.Veiculos;

    public List<Veiculos> Filtrar(Func<Veiculos, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();
}