using AventStack.ExtentReports;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Diagnostics;
using WebUiAutomation.HW3;
using WebUiAutomation.HW3.PageObjects;

namespace WebUiAutomation.HW5;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class HW5nUnit
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
    private static ExtentReports Extent;
    private ExtentTest Test;
    private Stopwatch _timer;

[OneTimeSetUp]
    public static void SetupReport()
    {
        Extent = ExtentManager.GetExtent();
    }

    [SetUp]
    public async Task Setup()
    {
        _timer = Stopwatch.StartNew();
        Test = Extent.CreateTest(TestContext.CurrentContext.Test.Name);
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
    public static async Task ONTearDown()
    {
        await BrowerSinglton.CloseBrowser();
        Extent.Flush();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _mainPage.ClosePage();
        await _browserContext.DisposeAsync();

        _playwright.Dispose();
        _timer.Stop();
        Test.Info("Execution time: " + _timer.Elapsed);


        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        switch (status)
        {
            case TestStatus.Passed:
                Test.Pass("Test passed");
                break;

            case TestStatus.Failed:
                Test.Fail(message);
                break;

            case TestStatus.Skipped:
                Test.Skip(message);
                break;
        }
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
        Assert.Ignore("Ignore");
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
            StringAssert.Contains("aaaaaaaa", actualText);
        }
        else
        {
            Assert.AreEqual("aaaaaaaaa", actualText);
        }

        await _contactPage.ClosePage();
    }
}
