using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
using Microsoft.AspNetCore.Identity;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloGrupoDeAutomoveis;

[TestClass]
[TestCategory("Testes de Integra��o de Grupo")]
public class RepositorioGrupoEmOrmTests
{
    RepositorioGrupoDeAutomoveisEmOrm? repositorioGrupo;
    LocadoraDeVeiculosDbContext? dbContext;
    UserManager<Usuario>? userManager;
    RoleManager<Perfil>? roleManager;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioGrupo = new(dbContext);

        dbContext.GrupoDeAutomoveis.RemoveRange(dbContext.GrupoDeAutomoveis);

        usuario = new Usuario { UserName = "testuser", Email = "testuser@example.com" };
        await dbContext.Users.AddAsync(usuario);
        await dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public void Deve_Inserir_GrupoDeAutomoveis_Corretamente()
    {
        // Arrange
        var novoRegistro = new GrupoDeAutomoveis("Oi", 1, 1, 1);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioGrupo!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioGrupo.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado n�o deve ser nulo.");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais.");
    }

    [TestMethod]
    public void Deve_Editar_GrupoDeAutomoveis_Corretamente()
    {
        // Arrange        
        var registroOriginal = new GrupoDeAutomoveis("Oi", 1, 1, 1);
        registroOriginal.UsuarioId = usuario.Id;

        repositorioGrupo!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioGrupo.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.Nome = "Testando";

        // Act
        repositorioGrupo.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_GrupoDeAutomoveis_Corretamente()
    {
        // Arrange
        var registro = new GrupoDeAutomoveis("Oi", 1, 1, 1);
        registro.UsuarioId = usuario.Id;

        repositorioGrupo!.Inserir(registro);

        // Act
        repositorioGrupo.Excluir(registro);

        // Assert
        GrupoDeAutomoveis? registroSelecionado = repositorioGrupo.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        // Arrange
        List<GrupoDeAutomoveis> registrosParaInserir =
        [
            new("Oi",1,1,1),
            new("Tchau",2,2,2),
            new("Tudo bem?",3,3,3)
        ];

        foreach (GrupoDeAutomoveis registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            repositorioGrupo!.Inserir(registro);
        }

        // Act
        List<GrupoDeAutomoveis> registrosSelecionados = repositorioGrupo!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}