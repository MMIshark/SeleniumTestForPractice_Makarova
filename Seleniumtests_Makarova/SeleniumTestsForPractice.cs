using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Seleniumtests_Makarova;

public class SeleniumTestsForPractice
{
    public ChromeDriver driver;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--start-maximized","--disable-extentions");
        
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        
        Authorization(); 
    }
    
    [Test]
    public void PassAuthorization()
    {
        var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        var currentUrl = driver.Url;
        
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");
        news.Should().NotBeNull();
    }

    [Test]
    public void OpenCommunitiesOnBigScreen()
    {
        GoToCommunities();
        IsAtComunities();
    }

    [Test]
    public void OpenCommunitiesOnSmallScreen()
    {
        driver.Manage().Window.Size = new Size(1024, 768);
        
        var sidebarMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebarMenu.Click();
        
        GoToCommunities();
        IsAtComunities();
    }

    [Test]
    public void CreateCommunity()
    {
        GoToCommunities();

        var createNewCommunity = driver.FindElement(By.CssSelector("[class='sc-juXuNZ sc-ecQkzk WTxfS vPeNx']"));
        createNewCommunity.Click();

        var communityName = driver.FindElement(By.CssSelector("[placeholder='Название сообщества']"));
        communityName.SendKeys("Автотест прошел :)");

        var create = driver.FindElement(By.CssSelector("[class='react-ui-j884du react-ui-button-caption']"));
        create.Click();
        
        var titleEditCommunity = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        titleEditCommunity.Should().NotBeNull();
        
        //Очистка после теста
        var deleteCommunity = driver.FindElement(By.CssSelector("[data-tid='DeleteButton']"));
        deleteCommunity.Click();
        
        var delete = driver.FindElement(By.CssSelector("[class='react-ui-j884du react-ui-button-caption']"));
        delete.Click();
    }

    [Test]
    public void ViewAndEditUserPage()
    {
        var goToProfile = driver.FindElement(By.CssSelector("[href='/profile/09026074-b53b-4f06-897b-b12158d435a5']"));
        goToProfile.Click();
        
        var editProfile = driver.FindElement(By.CssSelector("[class='sc-juXuNZ eiJUrb']"));
        editProfile.Click();

        var editModeOn = driver.FindElement(By.CssSelector("[data-tid='UploadFiles']"));
        var currentUrl = driver.Url;
        editModeOn.Should().NotBeNull();
        currentUrl.Should().Match("https://staff-testing.testkontur.ru/profile/settings/edit");
        //Поскольку тестирую не со своего профиля - ничего не редактирую, просто открываю режим редактирования.
    }
    
    public void Authorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("user");
        
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("1q2w3e4r%T");
        
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
    }

    public void GoToCommunities()
    {
        var communities = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
        communities.Click();
    }

    public void IsAtComunities()
    {
        var communitiesTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        var currentUrl = driver.Url;
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/communities");
        communitiesTitle.Should().NotBeNull();
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}