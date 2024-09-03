using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracaoe;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infra.Orm.ModuloCliente;
using LocadoraDeVeiculos.Infra.Orm.ModuloCondutor;
using LocadoraDeVeiculos.Infra.Orm.ModuloConfiguracao;
using LocadoraDeVeiculos.Infra.Orm.ModuloFuncionario;
using LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;
using LocadoraDeVeiculos.Infra.Orm.ModuloTaxa;
using LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
namespace LocadoraDeVeiculos.WebApp;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        #region Injecao de Dependencia de Servico
        builder.Services.AddDbContext<LocadoraDeVeiculosDbContext>();

        builder.Services.AddScoped<IRepositorioGrupoDeAutomoveis, RepositorioGrupoDeAutomoveisEmOrm>();
        builder.Services.AddScoped<IRepositorioVeiculo, RepositorioVeiculoEmOrm>();
        builder.Services.AddScoped<IRepositorioPlanoDeCobranca, RepositorioPlanoDeCobrancaEmOrm>();
        builder.Services.AddScoped<IRepositorioTaxa, RepositorioTaxaEmOrm>();
        builder.Services.AddScoped<IRepositorioCliente, RepositorioClienteEmOrm>();
        builder.Services.AddScoped<IRepositorioCondutor, RepositorioCondutorEmOrm>();
        builder.Services.AddScoped<IRepositorioConfiguracao, RepositorioConfiguracaoEmOrm>();
        builder.Services.AddScoped<IRepositorioAluguel, RepositorioAluguelEmOrm>();
        builder.Services.AddScoped<IRepositorioFuncionario, RepositorioFuncionarioEmOrm>();

        builder.Services.AddScoped<GrupoDeAutomoveisService>();
        builder.Services.AddScoped<VeiculoService>();
        builder.Services.AddScoped<PlanoDeCobrancaService>();
        builder.Services.AddScoped<TaxaService>();
        builder.Services.AddScoped<ClienteService>();
        builder.Services.AddScoped<CondutorService>();
        builder.Services.AddScoped<ConfiguracaoService>();
        builder.Services.AddScoped<AluguelService>();
        builder.Services.AddScoped<FuncionarioService>();

        builder.Services.AddAutoMapper( cfg => { cfg.AddMaps(Assembly.GetExecutingAssembly()); });

        #region Login
        builder.Services.AddIdentity<Usuario, Perfil>()
              .AddEntityFrameworkStores<LocadoraDeVeiculosDbContext>()
              .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 1;
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "AspNetCore.Cookies";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
            });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Usuario/Login";
            options.AccessDeniedPath = "/Usuario/AcessoNegado";
        });
        #endregion

        #endregion

        var app = builder.Build();

        if (!app.Environment.IsDevelopment()) app.UseHsts();

        app.UseHttpsRedirection();        
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}