using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
namespace LocadoraDeVeiculos.Testes.Unidade.ModuloFuncionario
{
    [TestClass]
    [TestCategory("Testes de Unidade de Funcionario")]
    public class FuncionarioTests
    {
        [TestMethod]
        public void Deve_Validar_Funcionario_Fisico_Corretamente()
        {
            var registroInvalido = new Funcionario("", DateTime.Now.AddDays(-1), 0, "");

            List<string> errosEsperados =
            [
                "O campo \"Nome\" é obrigatório. Tente novamente ",
                "O campo \"Login\" é obrigatório. Tente novamente ",
                "A data de admissão deve ser inferior ao dia de hoje",
                "O campo \"Salário\" é obrigatório. Tente novamente ",
            ];

            List<string> erros = registroInvalido.Validar();

            CollectionAssert.AreEqual(errosEsperados, erros);
        }
    }
}
