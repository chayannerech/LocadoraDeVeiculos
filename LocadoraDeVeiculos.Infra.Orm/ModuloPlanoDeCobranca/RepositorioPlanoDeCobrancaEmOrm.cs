using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobrancas;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;IRepositorioPlanoDeCobrancas
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;

public class RepositorioPlanoDeCobrancasEmOrm : RepositorioBaseEmOrm<PlanoDeCobrancas>, IRepositorioPlanoDeCobrancas
{
    public RepositorioPlanoDeCobrancasEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<PlanoDeCobranca> ObterRegistros() 
        => _dbContext.PlanoDeCobranca;

    public List<PlanoDeCobrancas> Filtrar(Func<PlanoDeCobrancas, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();
}