using Microsoft.Playwright;
using NUnit.Framework;
using WebUiAutomation.HW3.PageObjects;

namespace WebUiAutomation.HW3;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class HW3nUnit
{
    private const string CookieButtonsSelector = ".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn";
    private const string LithuaniaVersionSite = @"https://lt.ehuniversity.lt/";
    private const string AboutUrl = "https://en.ehuniversity.lt/about/";
    private const string StudyProgramPath = "/?s=study+programs";
    private const string About = "About";
    private const string InputSearchProgram = "study programs";
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _browserContext;
    private MainPageObject _mainPage;
    private AboutPageObject _aboutPage;
    private ContactPageObject _contactPage;

    [SetUp]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await BrowerSinglton.GetBrowser(_playwright);
        _browserContext = await _browser.NewContextAsync();
        var page = await _browserContext.NewPageAsync();

        await page.GotoAsync(TestSettings.MainUrl);

        var cookieButton = await page.QuerySelectorAsync(CookieButtonsSelector);
        if (cookieButton != null)
        {
            await cookieButton.ClickAsync();
        }

        _mainPage = new MainPageObject(page);
    }

    [OneTimeTearDown]
    public async Task ONTearDown()
    {
        await BrowerSinglton.CloseBrowser();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _mainPage.ClosePage();
        await _browserContext.DisposeAsync();

        _playwright.Dispose();
    }

    [Test]
    [Category("Language")]
    public async Task ChangeLanguage()
    {
        await _mainPage.ClickOnChooseLanguage();
        await _mainPage.ClickOnLithuanianLanguage();

        StringAssert.Contains(LithuaniaVersionSite, _mainPage.GetPageUrl());
    }

    [Test]
    [Category("Search")]
    public async Task SearchResults()
    {
        await _mainPage.ClickSearchHeader();
        await _mainPage.FillInputSearchProgramAsync(InputSearchProgram);
        await _mainPage.ClickSearchButton();

        StringAssert.Contains(StudyProgramPath, _mainPage.GetPageUrl());
    }

    [Test]
    [Category("Navigation")]
    public async Task VerifyAboutPageNavigation()
    {
        await _mainPage.GoToAboutPageUsingFooter();
        _aboutPage = new AboutPageObject(_mainPage.GetPage());

        Assert.AreEqual(AboutUrl, _aboutPage.GetPageUrl());
        StringAssert.Contains(About, await _aboutPage.GetTitleAboutAsync());

        await _aboutPage.ClosePage();
    }

    [Category("Contact")]
    [TestCase("a[plerdy-tracking-id='35448735101']", "franciskscarynacr@gmail.com")]
    [TestCase("li[plerdy-tracking-id='50296369501']", "+370 68 771365")]
    [TestCase("li[plerdy-tracking-id='39744896801']", "+375 29 5781488")]
    [TestCase("li[plerdy-tracking-id='64965466401']", "Join us in the social networks: Facebook Telegram VK")]
    public async Task VerifyContactInfo(string selector, string expectedText)
    {
        await _mainPage.GoToContactPage();
        _contactPage = new ContactPageObject(_mainPage.GetPage());

        await _contactPage.WaitUntilContactInfoVisiable(selector);

        var actualText = await _contactPage.GetTextBySelector(selector);

        if (expectedText.StartsWith('+'))
        {
            StringAssert.Contains(expectedText, actualText);
        }
        else
        {
            Assert.AreEqual(expectedText, actualText);
        }

        await _contactPage.ClosePage();
    }
}
