using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;

public class RepositorioPlanoDeCobrancaEmOrm : RepositorioBaseEmOrm<PlanoDeCobranca>, IRepositorioPlanoDeCobranca
{
    public RepositorioPlanoDeCobrancaEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<PlanoDeCobranca> ObterRegistros() 
        => _dbContext.PlanosDeCobranca;

    public List<PlanoDeCobranca> Filtrar(Func<PlanoDeCobranca, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();
}