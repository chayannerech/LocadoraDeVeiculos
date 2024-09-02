using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloFuncionario;
public class RepositorioFuncionarioEmOrm : RepositorioBaseEmOrm<Funcionario>, IRepositorioFuncionario
{
    public RepositorioFuncionarioEmOrm(LocadoraDeVeiculosDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Funcionario> ObterRegistros()
        => _dbContext.Funcionarios;

    public List<Funcionario> Filtrar(Func<Funcionario, bool> predicate)
        => _dbContext.Funcionarios
            .Where(predicate)
            .ToList();
}