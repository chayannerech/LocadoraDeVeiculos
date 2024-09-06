using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;
using Microsoft.AspNetCore.Identity;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloPlanoDeCobranca;

[TestClass]
[TestCategory("Testes de Integração de Plano")]
public class RepositorioGrupoEmOrmTests
{
    RepositorioPlanoDeCobrancaEmOrm? repositorioGrupo;
    LocadoraDeVeiculosDbContext? dbContext;
    UserManager<Usuario>? userManager;
    RoleManager<Perfil>? roleManager;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioGrupo = new(dbContext);

        dbContext.PlanosDeCobranca.RemoveRange(dbContext.PlanosDeCobranca);

        usuario = new Usuario { UserName = "testuser", Email = "testuser@example.com" };
        await dbContext.Users.AddAsync(usuario);
        await dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public void Deve_Inserir_PlanoDeCobranca_Corretamente()
    {
        // Arrange
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var novoRegistro = new PlanoDeCobranca(grupo, 10,10,10,10,10,10);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioGrupo!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioGrupo.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_PlanoDeCobranca_Corretamente()
    {
        // Arrange        
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var registroOriginal = new PlanoDeCobranca(grupo, 10, 10, 10, 10, 10, 10);
        registroOriginal.UsuarioId = usuario.Id;

        repositorioGrupo!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioGrupo.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.PrecoKm = 20;

        // Act
        repositorioGrupo.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_PlanoDeCobranca_Corretamente()
    {
        // Arrange
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var registro = new PlanoDeCobranca(grupo, 10, 10, 10, 10, 10, 10);
        registro.UsuarioId = usuario.Id;

        repositorioGrupo!.Inserir(registro);

        // Act
        repositorioGrupo.Excluir(registro);

        // Assert
        PlanoDeCobranca? registroSelecionado = repositorioGrupo.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        // Arrange
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        List<PlanoDeCobranca> registrosParaInserir =
        [
            new(grupo, 10, 10, 10, 10, 10, 10),
            new(grupo, 20, 10, 10, 10, 10, 10),
            new(grupo, 30, 10, 10, 10, 10, 10)
        ];

        foreach (PlanoDeCobranca registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            repositorioGrupo!.Inserir(registro);
        }

        // Act
        List<PlanoDeCobranca> registrosSelecionados = repositorioGrupo!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}