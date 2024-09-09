using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloAluguel;
public class RepositorioAluguelEmOrm : RepositorioBaseEmOrm<Aluguel>, IRepositorioAluguel
{
    public RepositorioAluguelEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Aluguel> ObterRegistros() 
        => _dbContext.Alugueis;

    public override Aluguel? SelecionarPorId(int id)
        => ObterRegistros()
            .Where(a => a.Ativo)
            .Include(a => a.Cliente)
            .Include(a => a.Condutor)
            .Include(a => a.Grupo)
            .Include(a => a.Plano)
            .Include(a => a.Veiculo)
            .Include(a => a.Configuracao)
            .Include(a => a.Funcionario)
            .FirstOrDefault(a => a.Id == id);

    public override List<Aluguel> SelecionarTodos()
        => [.. ObterRegistros()
            .Where(a => a.Ativo)
            .Include(c => c.Cliente)
            .Include(a => a.Condutor)
            .Include(a => a.Grupo)
            .Include(a => a.Plano)
            .Include(a => a.Veiculo)
            .Include(a => a.Configuracao)
            .Include(a => a.Funcionario)];

    public override List<Aluguel> Filtrar(Func<Aluguel, bool> predicate)    
        => ObterRegistros()
            .Where(a => a.Ativo)
            .Include(c => c.Cliente)
            .Include(a => a.Condutor)
            .Include(a => a.Grupo)
            .Include(a => a.Plano)
            .Include(a => a.Veiculo)
            .Include(a => a.Configuracao)
            .Include(a => a.Funcionario)
            .Where(predicate)
            .ToList();
}