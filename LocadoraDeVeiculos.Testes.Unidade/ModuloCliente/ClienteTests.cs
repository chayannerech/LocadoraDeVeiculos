using LocadoraDeVeiculos.Dominio.ModuloCliente;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloCliente
{
    [TestClass]
    [TestCategory("Testes de Unidade de Cliente")]
    public class ClienteTests
    {
        [TestMethod]
        public void Deve_Validar_Cliente_Fisico_Corretamente()
        {
            var registroInvalido = new Cliente(true, "", "", "", "", "", "", "", "", "", "", 0);

            List<string> errosEsperados =
            [
                "O campo \"Nome\" é obrigatório. Tente novamente ",
                "O campo \"Email\" é obrigatório. Tente novamente ",
                "O campo \"Telefone\" é obrigatório. Tente novamente ",
                "O campo \"Documento\" é obrigatório. Tente novamente ",
                "O campo \"RG\" é obrigatório. Tente novamente ",
                "O campo \"CNH\" é obrigatório. Tente novamente ",
                "O campo \"Estado\" é obrigatório. Tente novamente ",
                "O campo \"Cidade\" é obrigatório. Tente novamente ",
                "O campo \"Bairro\" é obrigatório. Tente novamente ",
                "O campo \"Rua\" é obrigatório. Tente novamente ",
                "O campo \"Numero\" é obrigatório. Tente novamente ",
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }

        [TestMethod]
        public void Deve_Validar_Cliente_Juridico_Corretamente()
        {
            var registroInvalido = new Cliente(false, "", "", "", "", "", "", "", "", "", "", 0);

            List<string> errosEsperados =
            [
                "O campo \"Nome\" é obrigatório. Tente novamente ",
                "O campo \"Email\" é obrigatório. Tente novamente ",
                "O campo \"Telefone\" é obrigatório. Tente novamente ",
                "O campo \"Documento\" é obrigatório. Tente novamente ",
                "O campo \"Estado\" é obrigatório. Tente novamente ",
                "O campo \"Cidade\" é obrigatório. Tente novamente ",
                "O campo \"Bairro\" é obrigatório. Tente novamente ",
                "O campo \"Rua\" é obrigatório. Tente novamente ",
                "O campo \"Numero\" é obrigatório. Tente novamente ",
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
