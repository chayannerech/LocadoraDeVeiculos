using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;

public class RepositorioPlanoDeCobrancasEmOrm : RepositorioBaseEmOrm<PlanoDeCobranca>, IRepositorioPlanoDeCobranca
{
    public RepositorioPlanoDeCobrancasEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<PlanoDeCobranca> ObterRegistros() 
        => _dbContext.PlanoDeCobranca;

    public List<PlanoDeCobranca> Filtrar(Func<PlanoDeCobranca, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();
}