using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
public class RepositorioGrupoDeAutomoveisEmOrm : RepositorioBaseEmOrm<GrupoDeAutomoveis>, IRepositorioGrupoDeAutomoveis
{
    public RepositorioGrupoDeAutomoveisEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<GrupoDeAutomoveis> ObterRegistros()
        => _dbContext.GrupoDeAutomoveis;
}