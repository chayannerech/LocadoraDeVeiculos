using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;

public class RepositorioVeiculosEmOrm : RepositorioBaseEmOrm<Veiculos>, IRepositorioVeiculos
{
    public RepositorioVeiculosEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Veiculos> ObterRegistros() 
        => _dbContext.Veiculos;

    public Veiculos? SelecionarPorId(int id)
    {
        return _dbContext.Veiculos
            .Include(s => s.GrupoDeAutomoveis)
            .FirstOrDefault(s => s.Id == id);
    }

    public List<Veiculos> SelecionarTodos()
        => [.. _dbContext.Veiculos
            .Include(s => s.GrupoDeAutomoveis)
            .AsNoTracking()];

    public List<Veiculos> Filtrar(Func<Veiculos, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();

    public List<IGrouping<string, Veiculos>> ObterVeiculosAgrupadosPorGrupo(int usuarioId)
        => [.. _dbContext.Veiculos
            //.Where(s => s.UsuarioId == usuarioId)
            .Include(s => s.GrupoDeAutomoveis)
            .GroupBy(s => s.GrupoDeAutomoveis.Nome)
            .AsNoTracking()];
    public List<IGrouping<string, Veiculos>> ObterVeiculosAgrupadosPorGrupo()
        => [.. _dbContext.Veiculos
            .Include(s => s.GrupoDeAutomoveis)
            .GroupBy(s => s.GrupoDeAutomoveis.Nome)
            .AsNoTracking() ];
}