using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloTaxa;

public class RepositorioTaxaEmOrm : RepositorioBaseEmOrm<Taxa>, IRepositorioTaxa
{
    public RepositorioTaxaEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Taxa> ObterRegistros() 
        => _dbContext.Taxas;

    public List<Taxa> Filtrar(Func<Taxa, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();
}