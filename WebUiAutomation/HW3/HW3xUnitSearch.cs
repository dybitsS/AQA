using Microsoft.Playwright;
using NUnit.Framework;
using WebUiAutomation.HW3;

namespace WebUiAutomationNunit;

[Parallelizable(ParallelScope.Children)]
[TestFixture]
public class HW3nUnit
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _browserContext;
    private SearchProgramPage _searchProgram;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await BrowerSinglton.GetBrowser(_playwright);
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await BrowerSinglton.CloseBrowser();
        _playwright.Dispose();
    }

    [Test]
    [Category("Language")]
    public async Task ChangeLanguage()
    {
        var page = await InitPage();
        var languageChoose = page.Locator("//li[@plerdy-tracking-id='39047282601']");
        await page.Locator("li[plerdy-tracking-id='41154587901']").ClickAsync();
        await languageChoose.WaitForAsync();
        await languageChoose.ClickAsync();

        StringAssert.Contains(@"https://lt.ehuniversity.lt/", page.Url);
        await page.CloseAsync();
    }

    [Test]
    [Category("Search")]
    public async Task SearchResults()
    {
        var page = await InitPage();
        await _searchProgram.SearchProgramAsync("study programs");
        StringAssert.Contains("/?s=study+programs", page.Url);
        await page.CloseAsync();
    }

    [Test]
    [Category("Navigation")]
    public async Task VerifyAboutPageNavigation()
    {
        var page = await InitPage();
        var footerAbout = page.Locator("//footer//a[contains(text(), 'About')]");
        await footerAbout.WaitForAsync();
        await footerAbout.ClickAsync();

        Assert.AreEqual("https://en.ehuniversity.lt/about/", page.Url);
        var title = await page.TitleAsync();
        StringAssert.Contains("About", title);
        await page.CloseAsync();
    }

    [Category("Contact")]
    [TestCase("a[plerdy-tracking-id='35448735101']", "franciskscarynacr@gmail.com")]
    [TestCase("li[plerdy-tracking-id='50296369501']", "+370 68 771365")]
    [TestCase("li[plerdy-tracking-id='39744896801']", "+375 29 5781488")]
    [TestCase("li[plerdy-tracking-id='64965466401']", "Join us in the social networks: Facebook Telegram VK")]
    public async Task VerifyContactInfo(string selector, string expectedText)
    {
        var page = await InitPage();
        await page.GotoAsync("https://en.ehu.lt/contact");
        var locator = page.Locator(selector);
        Assert.True(await locator.IsVisibleAsync());

        var actualText = await locator.InnerTextAsync();

        if (expectedText.StartsWith("+"))
        {
            StringAssert.Contains(expectedText, actualText);
        }
        else
        {
            Assert.AreEqual(expectedText, actualText);
        }
        await page.CloseAsync();
    }


    private async Task<IPage> InitPage()
    {
        _browserContext = await _browser.NewContextAsync();
        var page = await _browserContext.NewPageAsync();

        await page.GotoAsync("https://en.ehu.lt/");

        var cookieButton = await page.QuerySelectorAsync(".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn");
        if (cookieButton != null)
        {
            await cookieButton.ClickAsync();
        }

        _searchProgram = new SearchProgramPage(page);
        return page;
    }
}
