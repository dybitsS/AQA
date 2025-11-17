using Microsoft.Playwright;
using NUnit.Framework;
using Serilog;
using Shouldly;
using WebUiAutomation.HW3;
using WebUiAutomation.HW3.PageObjects;

namespace WebUiAutomation.HW4;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class HW4nUnit
{
    private const string CookieButtonsSelector = ".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn";
    private const string StudyProgramPath = "/?s=study+programs";
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
        Log.Debug("Creating Playwright");
        _playwright = await Playwright.CreateAsync();
        Log.Debug("Creating Browser");
        _browser = await BrowerSinglton.GetBrowser(_playwright);
        Log.Debug("Creating Browser Context");
        _browserContext = await _browser.NewContextAsync();
        Log.Debug("Open new page");
        var page = await _browserContext.NewPageAsync();

        Log.Debug($"Open {TestSettings.MainUrl} URL");
        await page.GotoAsync(TestSettings.MainUrl);

        Log.Debug($"Accept cookie");
        var cookieButton = await page.QuerySelectorAsync(CookieButtonsSelector);
        if (cookieButton != null)
        {
            await cookieButton.ClickAsync();
        }

        Log.Debug($"Create {nameof(MainPageObject)}");
        _mainPage = new MainPageObject(page);
    }

    [OneTimeSetUp]
    public static void Init()
    {
        Log.Logger = new LoggerConfiguration()
                   .Enrich.WithThreadId()
                   .MinimumLevel.Debug()
                   .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] ({ThreadId}) {Message:lj}{NewLine}{Exception}")
                   .WriteTo.File(
                        path: "logs/log-.txt",
                        rollingInterval: RollingInterval.Day,  
                        fileSizeLimitBytes: 10_000_000,  
                        rollOnFileSizeLimit: true,        
                        shared: true,                       
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] ({ThreadId}) {Message:lj}{NewLine}{Exception}")
                   .CreateLogger();
    }

    [OneTimeTearDown]
    public static async Task OneTearDown()
    {
        Log.Debug("Close browser");
        await BrowerSinglton.CloseBrowser();
        await Log.CloseAndFlushAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        Log.Debug($"Close page");
        await _mainPage.ClosePage();
        Log.Debug($"Dispose browser context");
        await _browserContext.DisposeAsync();

        Log.Debug($"Dispose Playwright");
        _playwright.Dispose();
    }

    [Test]
    [Category("Language")]
    public async Task ChangeLanguage()
    {
        try
        {
            Log.Information($"Entering method {nameof(ChangeLanguage)}");

            Log.Information("Click on menu to choose language");
            await _mainPage.ClickOnChooseLanguage();

            Log.Information("Click on lithuanian language");
            await _mainPage.ClickOnLithuanianLanguage();

            _mainPage.GetPageUrl().ShouldContain("pppppppppppppppp", Case.Insensitive);

            Log.Information("Test Passed");

        }
        catch (Exception ex)
        {
            Log.Information("Test Fail");
            Log.Error($"Message: {ex.Message} Stack Trace: {ex.StackTrace}");
            await _mainPage.GetPage().ScreenshotAsync(new PageScreenshotOptions()
            {
                Path = $"logs/screenshots/ChangeLanguage_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            });

            throw;
        }

        Log.Information($"Ending method {nameof(ChangeLanguage)}");
    }

    [Test]
    [Category("Search")]
    public async Task SearchResults()
    {
        try
        {
            Log.Information($"Entering method {nameof(SearchResults)}");

            Log.Information("Click on search header");
            await _mainPage.ClickSearchHeader();

            Log.Information("Fill field");
            await _mainPage.FillInputSearchProgramAsync(InputSearchProgram);

            Log.Information("Click on search button");
            await _mainPage.ClickSearchButton();

            _mainPage.GetPageUrl().ShouldContain(StudyProgramPath);
        }
        catch (Exception ex)
        {
            Log.Information("Test Fail");
            Log.Error($"Message: {ex.Message} Stack Trace: {ex.StackTrace}");
            await _mainPage.GetPage().ScreenshotAsync(new PageScreenshotOptions()
            {
                Path = $"ChangeLanguage_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            });
            throw;
        }

        Log.Information($"Ending method {nameof(SearchResults)}");
    }
}
