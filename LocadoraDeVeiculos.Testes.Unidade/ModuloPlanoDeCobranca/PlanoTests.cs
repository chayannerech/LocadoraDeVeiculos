using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloPlanoDeCobranca
{
    [TestClass]
    [TestCategory("Testes de Unidade de Plano de Cobrança")]
    public class PlanoTests
    {
        [TestMethod]
        public void Deve_Validar_Plano_Corretamente()
        {
            var registroInvalido = new PlanoDeCobranca(null, 0, 0, 0, 0, 0, 0);

            List<string> errosEsperados =
            [
                "O campo \"Grupo de Automóveis\" é obrigatório. Tente novamente ",
                "O campo \"Preço diário\" é obrigatório. Tente novamente ",
                "O campo \"Preço por Km\" é obrigatório. Tente novamente ",
                "O campo \"Quilometragem disponível\" é obrigatório. Tente novamente ",
                "O campo \"Preço da diária controlada\" é obrigatório. Tente novamente ",
                "O campo \"Preço por Km extrapolado\" é obrigatório. Tente novamente ",
                "O campo \"Preço livre\" é obrigatório. Tente novamente ",
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
