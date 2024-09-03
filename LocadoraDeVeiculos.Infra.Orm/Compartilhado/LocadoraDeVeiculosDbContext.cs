using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infra.Orm.ModuloCliente;
using LocadoraDeVeiculos.Infra.Orm.ModuloCondutor;
using LocadoraDeVeiculos.Infra.Orm.ModuloConfiguracao;
using LocadoraDeVeiculos.Infra.Orm.ModuloFuncionario;
using LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Infra.Orm.ModuloTaxa;
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
    public DbSet<Taxa> Taxas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Condutor> Condutores { get; set; }
    public DbSet<Configuracao> Configuracoes { get; set; }
    public DbSet<Aluguel> Alugueis { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }


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
        modelBuilder.ApplyConfiguration(new MapeadorTaxaEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorClienteEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorCondutorEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorConfiguracaoEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorAluguelEmOrm());
        modelBuilder.ApplyConfiguration(new MapeadorFuncionarioEmOrm());

        base.OnModelCreating(modelBuilder);
    }
}