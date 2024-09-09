using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloCondutor;

[TestClass]
[TestCategory("Testes de Integração de Condutor")]
public class RepositorioCondutorEmOrmTests
{
    RepositorioCondutorEmOrm? repositorioCondutor;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioCondutor = new(dbContext);

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
    public void Deve_Inserir_Condutor_Corretamente()
    {
        // Arrange
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        var novoRegistro = new Condutor(cliente, "oi", "", "", "", "", DateTime.Now);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioCondutor!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioCondutor.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_Condutor_Corretamente()
    {
        // Arrange        
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        var registroOriginal = new Condutor(cliente, "oi", "", "", "", "", DateTime.Now);
        registroOriginal.UsuarioId = usuario.Id;

        repositorioCondutor!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioCondutor.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.Nome = "Pedrinho";

        // Act
        repositorioCondutor.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_Condutor_Corretamente()
    {
        // Arrange
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        var registro = new Condutor(cliente, "oi", "", "", "", "", DateTime.Now);
        registro.UsuarioId = usuario.Id;

        repositorioCondutor!.Inserir(registro);

        // Act
        repositorioCondutor.Excluir(registro);

        // Assert
        Condutor? registroSelecionado = repositorioCondutor.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        // Arrange
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        List<Condutor> registrosParaInserir =
        [
            new(cliente, "oi", "", "", "", "", DateTime.Now),
            new(cliente, "oi", "", "", "", "", DateTime.Now),
            new(cliente, "oi", "", "", "", "", DateTime.Now)
        ];

        foreach (Condutor registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            repositorioCondutor!.Inserir(registro);
        }

        // Act
        List<Condutor> registrosSelecionados = repositorioCondutor!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}