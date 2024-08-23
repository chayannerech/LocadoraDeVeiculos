using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace LocadoraDeVeiculos.Infra.Orm.Compartilhado;
public class LocadoraDeVeiculosDbContext : IdentityDbContext<Usuario, Perfil, int>
{
    public DbSet<GrupoDeAutomoveis> GrupoDeAutomoveis { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<PlanoDeCobranca> PlanosDeCobranca { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config
            .GetConnectionString("SqlServer");

        optionsBuilder.UseSqlServer(connectionString);

        optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MapeadorGrupoDeAutomoveisEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorVeiculoEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorPlanoDeCobrancaEmOrm());

        base.OnModelCreating(modelBuilder);
    }
}