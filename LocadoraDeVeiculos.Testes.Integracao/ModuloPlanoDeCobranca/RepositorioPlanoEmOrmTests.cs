using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobrancas;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloPlanoDeCobranca;

[TestClass]
[TestCategory("Testes de Integração de Plano")]
public class RepositorioPLanoEmOrmTests
{
    RepositorioPlanoDeCobrancaEmOrm? repositorioPlano;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioPlano = new(dbContext);

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
    public void Deve_Inserir_PlanoDeCobranca_Corretamente()
    {
        // Arrange
        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var novoRegistro = new PlanoDeCobranca(grupo, 10,10,10,10,10,10);
        novoRegistro.UsuarioId = usuario.Id;

        // Act
        repositorioPlano!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioPlano.SelecionarPorId(novoRegistro.Id);

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

        repositorioPlano!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioPlano.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.PrecoKm = 20;

        // Act
        repositorioPlano.Editar(registroParaAtualizacao);

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

        repositorioPlano!.Inserir(registro);

        // Act
        repositorioPlano.Excluir(registro);

        // Assert
        PlanoDeCobranca? registroSelecionado = repositorioPlano.SelecionarPorId(registro.Id);

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
            repositorioPlano!.Inserir(registro);
        }

        // Act
        List<PlanoDeCobranca> registrosSelecionados = repositorioPlano!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}