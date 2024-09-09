using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using LocadoraDeVeiculos.Infra.Orm.ModuloAluguel;
namespace LocadoraDeVeiculos.Testes.Integracao.ModuloAluguel;

[TestClass]
[TestCategory("Testes de Integração de Aluguel")]
public class RepositorioAluguelEmOrmTests
{
    RepositorioAluguelEmOrm? repositorioAluguel;
    LocadoraDeVeiculosDbContext? dbContext;
    Usuario usuario;

    [TestInitialize]
    public async Task ConfigurarTestes()
    {
        dbContext = new();
        repositorioAluguel = new(dbContext);

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
    public void Deve_Inserir_Aluguel_Corretamente()
    {
        // Arrange
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        var condutor = new Condutor(cliente, "oi", "", "", "", "", DateTime.Now);
        condutor.UsuarioId = usuario.Id;

        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var plano = new PlanoDeCobranca(grupo, 10, 10, 10, 10, 10, 10);
        plano.UsuarioId = usuario.Id;

        var veiculo = new Veiculo("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0);
        veiculo.UsuarioId = usuario.Id;

        var configuracao = new Configuracao(0, 0, 0, 0);
        configuracao.UsuarioId = usuario.Id;

        var funcionario = new Funcionario("", DateTime.Now, 0, "");
        funcionario.UsuarioId = usuario.Id;

        var novoRegistro = new Aluguel(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 0, DateTime.Now, DateTime.Now, "", DateTime.Now, 0, 0, true, configuracao);
        novoRegistro.UsuarioId = usuario.Id;
        novoRegistro.Funcionario = funcionario;

        // Act
        repositorioAluguel!.Inserir(novoRegistro);

        // Assert
        var registroSelecionado = repositorioAluguel.SelecionarPorId(novoRegistro.Id);

        Assert.IsNotNull(registroSelecionado, "O registro selecionado não deve ser nulo");
        Assert.AreEqual(novoRegistro, registroSelecionado, "O registro inserido e o selecionado devem ser iguais");
    }

    [TestMethod]
    public void Deve_Editar_Aluguel_Corretamente()
    {
        // Arrange        
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        var condutor = new Condutor(cliente, "oi", "", "", "", "", DateTime.Now);
        condutor.UsuarioId = usuario.Id;

        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var plano = new PlanoDeCobranca(grupo, 10, 10, 10, 10, 10, 10);
        plano.UsuarioId = usuario.Id;

        var veiculo = new Veiculo("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0);
        veiculo.UsuarioId = usuario.Id;

        var configuracao = new Configuracao(0, 0, 0, 0);
        configuracao.UsuarioId = usuario.Id;

        var funcionario = new Funcionario("", DateTime.Now, 0, "");
        funcionario.UsuarioId = usuario.Id;

        var registroOriginal = new Aluguel(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 0, DateTime.Now, DateTime.Now, "", DateTime.Now, 0, 0, true, configuracao);
        registroOriginal.UsuarioId = usuario.Id;
        registroOriginal.Funcionario = funcionario;

        repositorioAluguel!.Inserir(registroOriginal);

        var registroParaAtualizacao = repositorioAluguel.SelecionarPorId(registroOriginal.Id);

        registroParaAtualizacao!.KmInicial = 50;

        // Act
        repositorioAluguel.Editar(registroParaAtualizacao);

        // Assert
        Assert.AreEqual(registroOriginal, registroParaAtualizacao);
    }

    [TestMethod]
    public void Deve_Excluir_Aluguel_Corretamente()
    {
        // Arrange
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        var condutor = new Condutor(cliente, "oi", "", "", "", "", DateTime.Now);
        condutor.UsuarioId = usuario.Id;

        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var plano = new PlanoDeCobranca(grupo, 10, 10, 10, 10, 10, 10);
        plano.UsuarioId = usuario.Id;

        var veiculo = new Veiculo("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0);
        veiculo.UsuarioId = usuario.Id;

        var configuracao = new Configuracao(0, 0, 0, 0);
        configuracao.UsuarioId = usuario.Id;

        var funcionario = new Funcionario("", DateTime.Now, 0, "");
        funcionario.UsuarioId = usuario.Id;

        var registro = new Aluguel(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 0, DateTime.Now, DateTime.Now, "", DateTime.Now, 0, 0, true, configuracao);
        registro.UsuarioId = usuario.Id;
        registro.Funcionario = funcionario;

        repositorioAluguel!.Inserir(registro);

        // Act
        repositorioAluguel.Excluir(registro);

        // Assert
        Aluguel? registroSelecionado = repositorioAluguel.SelecionarPorId(registro.Id);

        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Corretamente()
    {
        var cliente = new Cliente(true, "oi", "", "", "", "", "", "", "", "", "", 0);
        cliente.UsuarioId = usuario.Id;

        var condutor = new Condutor(cliente, "oi", "", "", "", "", DateTime.Now);
        condutor.UsuarioId = usuario.Id;

        var grupo = new GrupoDeAutomoveis("oi", 0, 0, 0);
        grupo.UsuarioId = usuario.Id;

        var plano = new PlanoDeCobranca(grupo, 10, 10, 10, 10, 10, 10);
        plano.UsuarioId = usuario.Id;

        var veiculo = new Veiculo("1", "2", "3", "4", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "5", grupo, 0);
        veiculo.UsuarioId = usuario.Id;

        var configuracao = new Configuracao(0, 0, 0, 0);
        configuracao.UsuarioId = usuario.Id;

        var funcionario = new Funcionario("", DateTime.Now, 0, "");
        funcionario.UsuarioId = usuario.Id;


        // Arrange
        List<Aluguel> registrosParaInserir =
        [
            new(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 0, DateTime.Now, DateTime.Now, "", DateTime.Now, 0, 0, true, configuracao),
            new(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 0, DateTime.Now, DateTime.Now, "", DateTime.Now, 0, 0, true, configuracao),
            new(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 0, DateTime.Now, DateTime.Now, "", DateTime.Now, 0, 0, true, configuracao)
        ];

        foreach (Aluguel registro in registrosParaInserir)
        {
            registro.UsuarioId = usuario.Id;
            registro.Funcionario = funcionario;
            repositorioAluguel!.Inserir(registro);
        }

        // Act
        List<Aluguel> registrosSelecionados = repositorioAluguel!.SelecionarTodos();

        // Assert
        CollectionAssert.AreEqual(registrosParaInserir, registrosSelecionados);
    }
}