using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloFuncionario;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloFuncionario;

[TestClass]
[TestCategory("Testes de Integração de Funcionario")]
public class RepositorioFuncionarioEmOrmTests
{
    RepositorioFuncionarioEmOrm? repositorioFuncionario;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioFuncionario = new(dbContext);

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
    public void Deve_Inserir_Funcionario_Corretamente()
    {
        // Arrange
        var novoRegistro = new Funcionario("", DateTime.Now, 0, "");
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioFuncionario!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioFuncionario.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_Funcionario_Corretamente()
    {
        // Arrange        
        var registroOriginal = new Funcionario("", DateTime.Now, 0, "");
        registroOriginal.UsuarioId = usuario.Id;

        repositorioFuncionario!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioFuncionario.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.Nome = "Pedrinho";

        // Act
        repositorioFuncionario.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_Funcionario_Corretamente()
    {
        // Arrange
        var registro = new Funcionario("", DateTime.Now, 0, "");
        registro.UsuarioId = usuario.Id;

        repositorioFuncionario!.Inserir(registro);

        // Act
        repositorioFuncionario.Excluir(registro);

        // Assert
        Funcionario? registroSelecionado = repositorioFuncionario.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        // Arrange
        List<Funcionario> registrosParaInserir =
        [
            new("", DateTime.Now, 0, ""),
            new("", DateTime.Now, 0, ""),
            new("", DateTime.Now, 0, "")
        ];

        foreach (Funcionario registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            repositorioFuncionario!.Inserir(registro);
        }

        // Act
        List<Funcionario> registrosSelecionados = repositorioFuncionario!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}