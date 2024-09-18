using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
namespace LocadoraDeVeiculos.Testes.Interface;

[TestClass]
public class AoRegistrarUsuario
{
    [TestMethod]
    public void Dado_info_validas_deve_apresentar_tela_inicial()
    {
        IWebDriver driver = new ChromeDriver();

        driver.Navigate().GoToUrl("http://localhost:5125/Usuario/Registrar");

        var inputUsuario = driver.FindElement(By.Id("usuario"));
        inputUsuario.SendKeys("Chay");
    }
}