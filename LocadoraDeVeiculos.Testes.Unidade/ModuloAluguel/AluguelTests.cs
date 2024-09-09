using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
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
            var registroInvalido = new Aluguel(null, null, null, null, CategoriaDePlanoEnum.Diário, null, 0, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), "", new DateTime(1950, 1, 1), 0, 0, false, null);

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

        [TestMethod]
        public void Deve_Validar_Devolucao_Aluguel_Corretamente()
        {
            var registroInvalido = new Aluguel(null, null, null, null, CategoriaDePlanoEnum.Diário, null, 0, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "", new DateTime(1950, 2, 1), 0, 0, false, null);

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
            var cliente = new Cliente(true, "", "", "", "", "", "", "", "", "", "", 0);
            var condutor = new Condutor(cliente, "", "", "", "", "", DateTime.Now);
            var grupo = new GrupoDeAutomoveis("", 100, 200, 300);
            var veiculo = new Veiculo("", "", "", "", TipoDeCombustivelEnum.Gasolina, 0, 0, [], "", grupo, 50);
            var plano = new PlanoDeCobranca(grupo, 100, 2, 200, 200, 5, 300);
            var funcionario = new Funcionario("", DateTime.Now, 0, "");
            var registroTeste = new Aluguel(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 1000, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), "", new DateTime(1950, 2, 1), 0, 50, false, null)
            {
                Taxas = [
                    new("Seguro", 50, false), 
                    new("Limpeza", 100, true), 
                    new("Frigobar", 100, false)]
            };

            var valor = registroTeste.CalcularValorTotalRetirada();

            Assert.AreEqual(valor, 1600);
        }

        [TestMethod]
        public void Deve_Calcular_Valor_De_Devolucao_Corretamente()
        {
            var cliente = new Cliente(true, "", "", "", "", "", "", "", "", "", "", 0);
            var condutor = new Condutor(cliente, "", "", "", "", "", DateTime.Now);
            var grupo = new GrupoDeAutomoveis("", 100, 200, 300);
            var veiculo = new Veiculo("", "", "", "", TipoDeCombustivelEnum.Gasolina, 50, 0, [], "", grupo, 50);
            var plano = new PlanoDeCobranca(grupo, 100, 2, 200, 200, 5, 300);
            var funcionario = new Funcionario("", DateTime.Now, 0, "");
            var configuracao = new Configuracao(5,4,3,2);
            var registroTeste = new Aluguel(condutor, cliente, grupo, plano, CategoriaDePlanoEnum.Diário, veiculo, 1000, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "", DateTime.Now.AddDays(3), 0, 50, false, configuracao)
            {
                Taxas = [
                    new("Seguro", 50, false),
                    new("Limpeza", 100, true),
                    new("Frigobar", 100, false)]
            };

            var valor = registroTeste.CalcularValorTotalDevolucao();

            Assert.AreEqual(valor, 2145);
        }
    }
}
