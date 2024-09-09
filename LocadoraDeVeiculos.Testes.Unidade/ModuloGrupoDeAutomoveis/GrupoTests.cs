using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloGrupoDeAutomoveis
{
    [TestClass]
    [TestCategory("Testes de Unidade de Grupo")]
    public class GrupoTests
    {
        [TestMethod]
        public void Deve_Validar_Grupo_Corretamente()
        {
            var registroInvalido = new GrupoDeAutomoveis("", 0, 0, 0);

            List<string> errosEsperados =
            [
                "O campo \"Nome\" é obrigatório. Tente novamente ",
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
