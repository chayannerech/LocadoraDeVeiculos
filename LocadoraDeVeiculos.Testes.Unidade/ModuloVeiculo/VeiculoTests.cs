using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloVeiculo
{
    [TestClass]
    [TestCategory("Testes de Unidade de Veiculo")]
    public class VeiculoTests
    {
        [TestMethod]
        public void Deve_Validar_Veiculo_Corretamente()
        {
            var registroInvalido = new Veiculo("", "", "", "", "", 0, 0, null, "", null);

            List<string> errosEsperados =
            [
                "O campo \"Placa\" é obrigatório. Tente novamente ",
                "O campo \"Marca\" é obrigatório. Tente novamente ",
                "O campo \"Cor\" é obrigatório. Tente novamente ",
                "O campo \"Modelo\" é obrigatório. Tente novamente ",
                "O campo \"Tipo de Combustível\" é obrigatório. Tente novamente ",
                "O campo \"Capacidade de Combustível\" é obrigatório. Tente novamente ",
                "O campo \"Ano\" é obrigatório. Tente novamente ",
                "O campo \"Foto\" é obrigatório. Tente novamente ",
                "O campo \"Foto\" é obrigatório. Tente novamente ",
                "O campo \"Grupo de Automóveis\" é obrigatório. Tente novamente ",
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }

        [TestMethod]
        public void Deve_Validar_Data_Corretamente()
        {
            var registroInvalido = new Veiculo("", "", "", "", "", 0, 2025, null, "", null);

            List<string> errosEsperados =
            [
                "O campo \"Placa\" é obrigatório. Tente novamente ",
                "O campo \"Marca\" é obrigatório. Tente novamente ",
                "O campo \"Cor\" é obrigatório. Tente novamente ",
                "O campo \"Modelo\" é obrigatório. Tente novamente ",
                "O campo \"Tipo de Combustível\" é obrigatório. Tente novamente ",
                "O campo \"Capacidade de Combustível\" é obrigatório. Tente novamente ",
                "O campo \"Foto\" é obrigatório. Tente novamente ",
                "O campo \"Foto\" é obrigatório. Tente novamente ",
                "O campo \"Grupo de Automóveis\" é obrigatório. Tente novamente ",
                "O ano precisa ser inferior a data atual. Tente novamente "
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
