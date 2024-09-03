using LocadoraDeVeiculos.Dominio.ModuloAluguel;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloAluguel
{
    [TestClass]
    [TestCategory("Testes de Unidade de Aluguel")]
    public class AluguelTests
    {
        [TestMethod]
        public void Deve_Validar_Saida_Aluguel_Corretamente()
        {
            var registroInvalido = new Aluguel(0,0,0,0,DateTime.Today.AddDays(-3), DateTime.MinValue, DateTime.MinValue);

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

    }
}
