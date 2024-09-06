using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloAluguel
{
    [TestClass]
    [TestCategory("Testes de Unidade de Aluguel")]
    public class AluguelTests
    {
        [TestMethod]
        public void Deve_Validar_Saida_Aluguel_Corretamente()
        {
            var cliente = new Cliente(true, "", "", "", "", "", "", "", "", "", "", 0);
            var condutor = new Condutor(cliente, "", "", "", "", "", DateTime.Now);
            var grupo = new GrupoDeAutomoveis("", 0, 0, 0);
            var veiculo = new Veiculo("", "", "", "", "", 0, 0, [], "", grupo);
            var plano = new PlanoDeCobranca(grupo, 0,0,0,0,0,0);
            var funcionario = new Funcionario("", DateTime.Now, 0, "");
            var registroInvalido = new Aluguel(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 0, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-3), "", DateTime.MinValue, 0, -10);

            List<string> errosEsperados =
            [
                "O campo \"Condutor\" é obrigatório. Tente novamente ",
                "O campo \"Cliente\" é obrigatório. Tente novamente ",
                "O campo \"Veiculo\" é obrigatório. Tente novamente ",
                "O veículo deve ser retirado hoje ou após o dia de hoje",
                "O veículo deve ser devolvido após a data de retirada"
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }

/*        [TestMethod]
        public void Deve_Validar_Devolucao_Aluguel_Corretamente()
        {
            var registroInvalido = new Aluguel(0, 0, 0, 0, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), DateTime.MinValue);

            List<string> errosEsperados =
            [
                "O campo \"Condutor\" é obrigatório. Tente novamente ",
                "O campo \"Cliente\" é obrigatório. Tente novamente ",
                "O campo \"Veiculo\" é obrigatório. Tente novamente ",
                "O veículo deve ser devolvido após a data de retirada"
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }

        [TestMethod]
        public void Deve_Calcular_Valor_De_Retirada_Corretamente()
        {
            var registroTeste = new Aluguel(1, 1, 1, 1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), DateTime.MinValue);

            var grupo = new GrupoDeAutomoveis("Caminhonete", 10, 20, 30);
            CategoriaDePlanoEnum categoria = CategoriaDePlanoEnum.Diário;
            List<Taxa> taxas = [new("Seguro", 50, false), new("Limpeza", 100, true), new("Frigobar", 100, false)];
            var dias = (registroTeste.DataRetornoPrevista - registroTeste.DataSaida).Days;

            var valor = registroTeste.CalcularValorTotalRetirada(grupo, categoria, taxas, dias, 1000);

            Assert.AreEqual(valor, 1420);
        }

        [TestMethod]
        public void Deve_Calcular_Valor_De_Devolucao_Corretamente()
        {
            var registroTeste = new Aluguel(1, 1, 1, 1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), DateTime.Today.AddDays(4));

            var grupo = new GrupoDeAutomoveis("Caminhonete", 10, 20, 30);
            var plano = new PlanoDeCobranca(grupo, 100,5,100,150,10,250);
            var veiculo = new Veiculo("", "", "", "", "Gasolina", 50, 2024, [], "", grupo);
            var configuracao = new Configuracao(5,4,3,4);
            CategoriaDePlanoEnum categoria = CategoriaDePlanoEnum.Diário;
            List<Taxa> taxas = [new("Seguro", 50, false), new("Limpeza", 100, true), new("Frigobar", 100, false)];
            var diasPrevistos = (registroTeste.DataRetornoPrevista - registroTeste.DataSaida).Days;
            var diasReais = (registroTeste.DataRetornoReal - registroTeste.DataSaida).Days;
            var kmInicial = 50;
            var kmAtual = 100;

            var valor = registroTeste.CalcularValorTotalDevolucao(plano, categoria, taxas, diasPrevistos, 1000, diasReais, kmInicial, kmAtual, false, veiculo, configuracao);

            Assert.AreEqual(valor, 2585);
        }
*/    }
}
