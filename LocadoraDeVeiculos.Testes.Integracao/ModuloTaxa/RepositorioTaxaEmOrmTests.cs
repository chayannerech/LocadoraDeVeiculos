using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloTaxa;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloTaxa;

[TestClass]
[TestCategory("Testes de Integração de Taxa")]
public class RepositorioTaxaEmOrmTests
{
    RepositorioTaxaEmOrm? repositorioTaxa;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioTaxa = new(dbContext);

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
    public void Deve_Inserir_Taxa_Corretamente()
    {
        // Arrange
        var novoRegistro = new Taxa("", 0, true);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioTaxa!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioTaxa.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_Taxa_Corretamente()
    {
        // Arrange        
        var registroOriginal = new Taxa("", 0, true);
        registroOriginal.UsuarioId = usuario.Id;

        repositorioTaxa!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioTaxa.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.Nome = "Limpeza";

        // Act
        repositorioTaxa.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_Taxa_Corretamente()
    {
        // Arrange
        var registro = new Taxa("", 0, true);
        registro.UsuarioId = usuario.Id;

        repositorioTaxa!.Inserir(registro);

        // Act
        repositorioTaxa.Excluir(registro);

        // Assert
        Taxa? registroSelecionado = repositorioTaxa.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        // Arrange
        List<Taxa> registrosParaInserir =
        [
            new("", 0, true),
            new("", 0, true),
            new("", 0, true)
        ];

        foreach (Taxa registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            repositorioTaxa!.Inserir(registro);
        }

        // Act
        List<Taxa> registrosSelecionados = repositorioTaxa!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}