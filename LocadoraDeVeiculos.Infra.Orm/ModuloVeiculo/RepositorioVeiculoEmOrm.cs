using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;
public class RepositorioVeiculoEmOrm : RepositorioBaseEmOrm<Veiculo>, IRepositorioVeiculo
{
    public RepositorioVeiculoEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Veiculo> ObterRegistros()
        => _dbContext.Veiculos;

    public override Veiculo? SelecionarPorId(int id)
        => ObterRegistros()
            .Where(v => v.Ativo)
            .Include(v => v.GrupoDeAutomoveis)
            .FirstOrDefault(v => v.Id == id);

    public override List<Veiculo> SelecionarTodos()
        => [.. ObterRegistros()
            .Where(v => v.Ativo)
            .Include(v => v.GrupoDeAutomoveis)];

    public List<IGrouping<string, Veiculo>> ObterVeiculosAgrupadosPorGrupo(int usuarioId)
        => [.. ObterRegistros()
            .Where(v => v.Ativo)
            .Where(v => v.UsuarioId == usuarioId)
            .Include(v => v.GrupoDeAutomoveis)
            .GroupBy(v => v.GrupoDeAutomoveis.Nome)
            .AsNoTracking()];

    public List<IGrouping<string, Veiculo>> ObterVeiculosAgrupadosPorGrupo()
        => [.. ObterRegistros()
            .Where(v => v.Ativo)
            .Include(v => v.GrupoDeAutomoveis)
            .GroupBy(v => v.GrupoDeAutomoveis.Nome)
            .AsNoTracking() ];
}