using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver;
    private const string BaseURL = "https://blazedemo.com";

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TeardownTest()
    {
        try
        {
            driver.Quit();
            driver.Dispose();
        }
        catch (Exception) { }
    }

    [Test]
    public void BlazeDemo_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        wait.Until(ExpectedConditions.ElementExists(By.Name("fromPort")));

        driver.FindElement(By.XPath("//select[@name='fromPort']/option[. = 'Mexico City']")).Click();
        driver.FindElement(By.XPath("//select[@name='toPort']/option[. = 'Dublin']")).Click();

        driver.FindElement(By.XPath("//input[@type='submit' and @value='Find Flights']")).Click();

        var flightRows = wait.Until(d =>
        {
            var rows = d.FindElements(By.XPath("//table[contains(@class,'table')]/tbody/tr"));
            return rows.Count > 0 ? rows : null;
        });

        // Assert
        flightRows.Count.Should().BeGreaterThanOrEqualTo(3, "at least 3 flights expected");
    }
}