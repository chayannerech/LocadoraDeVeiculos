using LocadoraDeVeiculos.Dominio.ModuloTaxa;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloTaxa
{
    [TestClass]
    [TestCategory("Testes de Unidade de Taxa")]
    public class TaxaTests
    {
        [TestMethod]
        public void Deve_Validar_Taxa_Corretamente()
        {
            var registroInvalido = new Taxa("", 0, false);

            List<string> errosEsperados =
            [
                "O campo \"Nome\" é obrigatório. Tente novamente ",
                "O campo \"Preço\" é obrigatório. Tente novamente ",
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
