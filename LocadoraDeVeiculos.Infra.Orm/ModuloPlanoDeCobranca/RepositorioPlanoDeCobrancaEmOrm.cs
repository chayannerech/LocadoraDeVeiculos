using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;
public class RepositorioPlanoDeCobrancaEmOrm : RepositorioBaseEmOrm<PlanoDeCobranca>, IRepositorioPlanoDeCobranca
{
    public RepositorioPlanoDeCobrancaEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<PlanoDeCobranca> ObterRegistros()
        => _dbContext.PlanosDeCobranca;

    public override PlanoDeCobranca? SelecionarPorId(int id)
        => ObterRegistros()
            .Where(p => p.Ativo)
            .Include(p => p.GrupoDeAutomoveis)
            .FirstOrDefault(p => p.Id == id);

    public override List<PlanoDeCobranca> SelecionarTodos()
        => [.. ObterRegistros()
            .Where(p => p.Ativo)
            .Include(p => p.GrupoDeAutomoveis)];

    public PlanoDeCobranca SelecionarPorGrupoId(int id)
        => ObterRegistros().FirstOrDefault(p => p.GrupoDeAutomoveis.Id == id && p.Ativo)!;
}