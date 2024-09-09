using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloGrupoDeAutomoveis;

[TestClass]
[TestCategory("Testes de Integração de Grupo")]
public class RepositorioGrupoEmOrmTests
{
    RepositorioGrupoDeAutomoveisEmOrm? repositorioGrupo;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioGrupo = new(dbContext);

        dbContext.GrupoDeAutomoveis.RemoveRange(dbContext.GrupoDeAutomoveis);
        dbContext.PlanosDeCobranca.RemoveRange(dbContext.PlanosDeCobranca);
        dbContext.Veiculos.RemoveRange(dbContext.Veiculos);

        dbContext.Clientes.RemoveRange(dbContext.Clientes);
        dbContext.Condutores.RemoveRange(dbContext.Condutores);

        dbContext.Configuracoes.RemoveRange(dbContext.Configuracoes);
        dbContext.Taxas.RemoveRange(dbContext.Taxas);

        dbContext.Alugueis.RemoveRange(dbContext.Alugueis);

        dbContext.Users.RemoveRange(dbContext.Users);
        dbContext.Funcionarios.RemoveRange(dbContext.Funcionarios);

        usuario = new Usuario { UserName = "testuser", Email = "testuser@example.com" };
        await dbContext.Users.AddAsync(usuario);
        dbContext.SaveChanges();
    }

    [TestMethod]
    public async Task Deve_Inserir_GrupoDeAutomoveis_Corretamente()
    {
        // Arrange
        var novoRegistro = new GrupoDeAutomoveis("Oi", 1, 1, 1);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioGrupo!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioGrupo.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo.");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais.");
    }

    [TestMethod]
    public async Task Deve_Editar_GrupoDeAutomoveis_Corretamente()
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