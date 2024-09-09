using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloConfiguracao;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloConfiguracao;

[TestClass]
[TestCategory("Testes de Integração de Configuracao")]
public class RepositorioConfiguracaoEmOrmTests
{
    RepositorioConfiguracaoEmOrm? repositorioConfiguracao;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioConfiguracao = new(dbContext);

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
    public void Deve_Inserir_Configuracao_Corretamente()
    {
        // Arrange
        var novoRegistro = new Configuracao(0, 0, 0, 0);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioConfiguracao!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioConfiguracao.Selecionar(usuario.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_Configuracao_Corretamente()
    {
        // Arrange        
        var registroOriginal = new Configuracao(0, 0, 0, 0);
        registroOriginal.UsuarioId = usuario.Id;

        repositorioConfiguracao!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioConfiguracao.Selecionar(usuario.Id);

        registroParaAtualizacao!.Gasolina = 10;

        // Act
        repositorioConfiguracao.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }
}