using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloCliente;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloCliente;

[TestClass]
[TestCategory("Testes de Integração de Cliente")]
public class RepositorioClienteEmOrmTests
{
    RepositorioClienteEmOrm? repositorioCliente;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioCliente = new(dbContext);

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
    public void Deve_Inserir_Cliente_Corretamente()
    {
        // Arrange
        var novoRegistro = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioCliente!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioCliente.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_Cliente_Corretamente()
    {
        // Arrange        
        var registroOriginal = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        registroOriginal.UsuarioId = usuario.Id;

        repositorioCliente!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioCliente.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.Nome = "Pedrinho";

        // Act
        repositorioCliente.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_Cliente_Corretamente()
    {
        // Arrange
        var registro = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        registro.UsuarioId = usuario.Id;

        repositorioCliente!.Inserir(registro);

        // Act
        repositorioCliente.Excluir(registro);

        // Assert
        Cliente? registroSelecionado = repositorioCliente.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        // Arrange
        List<Cliente> registrosParaInserir =
        [
            new(true, "oi", "", "", "", "", "", "", "", "", "", 0),
            new(true, "oi", "", "", "", "", "", "", "", "", "", 0),
            new(true, "oi", "", "", "", "", "", "", "", "", "", 0)
        ];

        foreach (Cliente registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            repositorioCliente!.Inserir(registro);
        }

        // Act
        List<Cliente> registrosSelecionados = repositorioCliente!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}