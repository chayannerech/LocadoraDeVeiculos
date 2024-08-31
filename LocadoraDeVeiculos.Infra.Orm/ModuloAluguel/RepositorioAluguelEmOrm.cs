using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloAluguel;

public class RepositorioAluguelEmOrm : RepositorioBaseEmOrm<Aluguel>, IRepositorioAluguel
{
    public RepositorioAluguelEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Aluguel> ObterRegistros() 
        => _dbContext.Alugueis;

    public Aluguel? SelecionarPorId(int id)
    {
        return _dbContext.Alugueis
            .FirstOrDefault(s => s.Id == id);
    }

    public List<Aluguel> SelecionarTodos()
        => [.. _dbContext.Alugueis
            .AsNoTracking()];

    public List<Aluguel> Filtrar(Func<Aluguel, bool> predicate)
        => ObterRegistros()
            .Where(predicate)
            .ToList();
}