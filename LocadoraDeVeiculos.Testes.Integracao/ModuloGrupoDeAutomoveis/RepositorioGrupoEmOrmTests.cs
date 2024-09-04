using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloGrupoDeAutomoveis;

[TestClass]
[TestCategory("Testes de Integração de Grupo")]
public class RepositorioGrupoEmOrmTests
{
    RepositorioGrupoDeAutomoveisEmOrm? repositorioGrupo;
    LocadoraDeVeiculosDbContext? dbContext;
    UserManager<Usuario>? userManager;
    RoleManager<Perfil>? roleManager;

    [TestInitialize]
    public void ConfigurarTestes()
    {
        dbContext = new();
        repositorioGrupo = new(dbContext);

        dbContext.GrupoDeAutomoveis.RemoveRange(dbContext.GrupoDeAutomoveis);
    }

    [TestMethod]
    public async Task Deve_Inserir_GrupoDeAutomoveis_Corretamente()
    {
        var usuario = new Usuario { UserName = "testuser", Email = "testuser@example.com" };

        // Arrange
        await dbContext.Users.AddAsync(usuario);
        await dbContext.SaveChangesAsync();

        var novoRegistro = new GrupoDeAutomoveis("Oi", 1, 1, 1)
        {
            UsuarioId = usuario.Id
        };

        // Act
        repositorioGrupo!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioGrupo.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo.");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais.");
    }
}

/*    [TestMethod]
    public void Deve_Editar_GrupoDeAutomoveis_Corretamente()
    {
        // Arrange
        var registroOriginal = new GrupoDeAutomoveis("Oi", 1,1,1);

        repositorioGrupo!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioGrupo.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.Nome = "Testando";

        // Act
        repositorioGrupo.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

*/    /*           [TestMethod]
               public void Deve_Excluir_GrupoDeAutomoveis_Corretamente()
               {
                   // Arrange
                   var content = "Este é o conteúdo do arquivo.";
                   var contentBytes = Encoding.UTF8.GetBytes(content);
                   var stream = new MemoryStream(contentBytes);
                   var imagem = new FormFile(stream, 0, stream.Length, "file", "example.txt")
                   {
                       Headers = new HeaderDictionary(),
                       ContentType = "text/plain"
                   };

                   GrupoDeAutomoveis filme = new("Oi", new TimeSpan(0, 0, 30), "Ação", imagem);

                   repositorioGrupo.Inserir(filme);

                   // Act
                   repositorioGrupo.Excluir(filme);

                   // Assert
                   GrupoDeAutomoveis registroSelecionado = repositorioGrupo.SelecionarPorId(filme.Id);

                   Assert.IsNull(registroSelecionado);
               }

               [TestMethod]
               public void Deve_Selecionar_Todos_Corretamente()
               {
                   // Arrange
                   var content = "Este é o conteúdo do arquivo.";
                   var contentBytes = Encoding.UTF8.GetBytes(content);
                   var stream = new MemoryStream(contentBytes);
                   var imagem = new FormFile(stream, 0, stream.Length, "file", "example.txt")
                   {
                       Headers = new HeaderDictionary(),
                       ContentType = "text/plain"
                   };

                   List<GrupoDeAutomoveis> filmesParaInserir =
                   [
                       new("Oi", new TimeSpan(0,0,30), "Ação", imagem),
                       new("Tchau", new TimeSpan(0,0,30), "Ação", imagem),
                       new("Tudo bem?", new TimeSpan(0,0,30), "Ação", imagem)
                   ];

                   foreach (GrupoDeAutomoveis filme in filmesParaInserir)
                       repositorioGrupo.Inserir(filme);

                   // Act
                   List<GrupoDeAutomoveis> filmesSelecionados = repositorioGrupo.SelecionarTodos();

                   // Assert
                   CollectionAssert.AreEqual(filmesParaInserir, filmesSelecionados);
               }*/