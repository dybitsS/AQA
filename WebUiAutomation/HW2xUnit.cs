using Microsoft.Playwright;
using WebUiAutomation.HW3;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads = 8)]
namespace WebUiAutomation;
public class HW2xUnitLanguageAndSearch : IClassFixture<PlaywrightFixture>, IDisposable
{
    private readonly PlaywrightFixture _fixture;
    private readonly IBrowserContext _context;
    private readonly IPage _page;

    public HW2xUnitLanguageAndSearch(PlaywrightFixture fixture)
    {
        _fixture = fixture;

        _context = _fixture.Browser.NewContextAsync().GetAwaiter().GetResult();
        _page = _context.NewPageAsync().GetAwaiter().GetResult();

        _page.GotoAsync("https://en.ehu.lt/").GetAwaiter().GetResult();

        var cookieButton = _page.QuerySelectorAsync(".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn")
            .GetAwaiter().GetResult();
        cookieButton?.ClickAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _context.CloseAsync().GetAwaiter().GetResult();
    }

    [Fact]
    [Trait("Language", "Search")]
    public async Task ChangeLanguage()
    {
        var languageChoose = _page.Locator("//li[@plerdy-tracking-id='39047282601']");
        await _page.Locator("li[plerdy-tracking-id='41154587901']").ClickAsync();
        await languageChoose.WaitForAsync();
        await languageChoose.ClickAsync();

        Assert.Equal(@"https://lt.ehuniversity.lt/", _page.Url);
    }

    [Fact]
    [Trait("Programs", "Search")]
    public async Task SearchResults()
    {
        await _page.Locator("//div[@class='header-search']").ClickAsync();
        var searchInput = _page.Locator("input[plerdy-tracking-id='30561103501']");
        await searchInput.FillAsync("study programs");
        await _page.Locator("button[plerdy-tracking-id='66179574101']").ClickAsync();
        Assert.Contains("/?s=study+programs", _page.Url);
    }
}

public class HW2xUnitVerifing : IClassFixture<PlaywrightFixture>, IDisposable
{
    private readonly PlaywrightFixture _fixture;
    private readonly IBrowserContext _context;
    private readonly IPage _page;

    public HW2xUnitVerifing(PlaywrightFixture fixture)
    {
        _fixture = fixture;

        _context = _fixture.Browser.NewContextAsync().GetAwaiter().GetResult();
        _page = _context.NewPageAsync().GetAwaiter().GetResult();

        _page.GotoAsync("https://en.ehu.lt/").GetAwaiter().GetResult();

        var cookieButton = _page.QuerySelectorAsync(".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn")
            .GetAwaiter().GetResult();
        cookieButton?.ClickAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _context.CloseAsync().GetAwaiter().GetResult();
    }

    [Fact]
    [Trait("Navigation", "About")]
    public async Task VerifyAboutPageNavigation()
    {
        var footerAbout = _page.Locator("//footer//a[contains(text(), 'About')]");
        await footerAbout.WaitForAsync();
        await footerAbout.ClickAsync();

        Assert.Equal("https://en.ehuniversity.lt/about/", _page.Url);
        var title = await _page.TitleAsync();
        Assert.Contains("About", title);
    }

    [Theory]
    [Trait("Navigation", "Contact")]
    [InlineData("a[plerdy-tracking-id='35448735101']", "franciskscarynacr@gmail.com")]
    [InlineData("li[plerdy-tracking-id='50296369501']", "+370 68 771365")]
    [InlineData("li[plerdy-tracking-id='39744896801']", "+375 29 5781488")]
    [InlineData("li[plerdy-tracking-id='64965466401']", "Join us in the social networks: Facebook Telegram VK")]
    public async Task VerifyContactInfo(string selector, string expectedText)
    {
        await _page.GotoAsync("https://en.ehu.lt/contact");
        var locator = _page.Locator(selector);
        Assert.True(await locator.IsVisibleAsync());

        var actualText = await locator.InnerTextAsync();

        if (expectedText.StartsWith("+"))
        {
            Assert.Contains(expectedText, actualText);
        }
        else
        {
            Assert.Equal(expectedText, actualText);
        }
    }

}

public class PlaywrightFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; }
    public IBrowser Browser { get; private set; }

    public async Task InitializeAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync();
    }

    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        Playwright.Dispose();
    }
}
