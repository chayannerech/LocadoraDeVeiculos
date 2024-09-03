using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;

public class RepositorioPlanoDeCobrancaEmOrm : RepositorioBaseEmOrm<PlanoDeCobranca>, IRepositorioPlanoDeCobranca
{
    public RepositorioPlanoDeCobrancaEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<PlanoDeCobranca> ObterRegistros() 
        => _dbContext.PlanosDeCobranca;

    public PlanoDeCobranca? SelecionarPorId(int id)
    {
        return _dbContext.PlanosDeCobranca
            .Include(s => s.GrupoDeAutomoveis)
            .FirstOrDefault(s => s.Id == id);
    }

    public List<PlanoDeCobranca> SelecionarTodos()
        => [.. _dbContext.PlanosDeCobranca
            .Include(s => s.GrupoDeAutomoveis)
            .AsNoTracking()];

    public List<PlanoDeCobranca> Filtrar(Func<PlanoDeCobranca, bool> predicate)
        => ObterRegistros()
            .Include(s => s.GrupoDeAutomoveis)
            .AsNoTracking()
            .Where(predicate)
            .ToList();

    public PlanoDeCobranca SelecionarPorGrupoId(int id)
        => _dbContext.PlanosDeCobranca.FirstOrDefault(x => x.GrupoDeAutomoveis.Id == id)!;
}