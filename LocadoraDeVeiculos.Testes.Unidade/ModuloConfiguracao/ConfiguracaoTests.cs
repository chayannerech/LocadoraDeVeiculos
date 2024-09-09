using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloConfiguracao
{
    [TestClass]
    [TestCategory("Testes de Unidade de Configuração")]
    public class ConfiguracaoTests
    {
        [TestMethod]
        public void Deve_Validar_Configuracao_Corretamente()
        {
            var registroInvalido = new Configuracao(0,0,0,0);

            List<string> errosEsperados =
            [
                "O campo \"Gasolina\" é obrigatório. Tente novamente ",
                "O campo \"Etanol\" é obrigatório. Tente novamente ",
                "O campo \"Diesel\" é obrigatório. Tente novamente ",
                "O campo \"GNV\" é obrigatório. Tente novamente "
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
