using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;

public class RepositorioVeiculoEmOrm : RepositorioBaseEmOrm<Veiculo>, IRepositorioVeiculo
{
    public RepositorioVeiculoEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Veiculo> ObterRegistros() 
        => _dbContext.Veiculos;

    public Veiculo? SelecionarPorId(int id)
    {
        return _dbContext.Veiculos
            .Include(s => s.GrupoDeAutomoveis)
            .FirstOrDefault(s => s.Id == id);
    }

    public List<Veiculo> SelecionarTodos()
        => [.. _dbContext.Veiculos
            .Include(s => s.GrupoDeAutomoveis)
            .AsNoTracking()];

    public List<Veiculo> Filtrar(Func<Veiculo, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();

    public List<IGrouping<string, Veiculo>> ObterVeiculosAgrupadosPorGrupo(int usuarioId)
        => [.. _dbContext.Veiculos
            //.Where(s => s.UsuarioId == usuarioId)
            .Include(s => s.GrupoDeAutomoveis)
            .GroupBy(s => s.GrupoDeAutomoveis.Nome)
            .AsNoTracking()];
    public List<IGrouping<string, Veiculo>> ObterVeiculosAgrupadosPorGrupo()
        => [.. _dbContext.Veiculos
            .Include(s => s.GrupoDeAutomoveis)
            .GroupBy(s => s.GrupoDeAutomoveis.Nome)
            .AsNoTracking() ];
}