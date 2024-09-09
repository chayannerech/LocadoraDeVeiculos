using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloVeiculo;

[TestClass]
[TestCategory("Testes de Integração de Veiculo")]
public class RepositorioGrupoEmOrmTests
{
    RepositorioVeiculoEmOrm? repositorioVeiculo;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioVeiculo = new(dbContext);

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
    public void Deve_Inserir_Veiculo_Corretamente()
    {
        // Arrange
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var novoRegistro = new Veiculo("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioVeiculo!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioVeiculo.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_Veiculo_Corretamente()
    {
        // Arrange        
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var registroOriginal = new Veiculo("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0);
        registroOriginal.UsuarioId = usuario.Id;

        repositorioVeiculo!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioVeiculo.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.Ano = 2024;

        // Act
        repositorioVeiculo.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_Veiculo_Corretamente()
    {
        // Arrange
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var registro = new Veiculo("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0);
        registro.UsuarioId = usuario.Id;

        repositorioVeiculo!.Inserir(registro);

        // Act
        repositorioVeiculo.Excluir(registro);

        // Assert
        Veiculo? registroSelecionado = repositorioVeiculo.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        // Arrange
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        List<Veiculo> registrosParaInserir =
        [
            new("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0),
            new("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0),
            new("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0)
        ];

        foreach (Veiculo registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            repositorioVeiculo!.Inserir(registro);
        }

        // Act
        List<Veiculo> registrosSelecionados = repositorioVeiculo!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}