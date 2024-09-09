using LocadoraDeVeiculos.Dominio.ModuloCondutor;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloCondutor
{
    [TestClass]
    [TestCategory("Testes de Unidade de Condutor")]
    public class CondutorTests
    {
        [TestMethod]
        public void Deve_Validar_Condutor_Corretamente()
        {
            var registroInvalido = new Condutor(null, "", "", "", "", "", DateTime.MinValue);

            List<string> errosEsperados =
            [
                "O campo \"Cliente\" é obrigatório. Tente novamente ",
                "O campo \"Nome\" é obrigatório. Tente novamente ",
                "O campo \"Email\" é obrigatório. Tente novamente ",
                "O campo \"Telefone\" é obrigatório. Tente novamente ",
                "O campo \"CPF\" é obrigatório. Tente novamente ",
                "A CNH não pode estar vencida"
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
