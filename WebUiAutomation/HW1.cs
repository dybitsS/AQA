using Microsoft.Playwright;
using Xunit;

namespace WebUiAutomation;
public class HW1
{
    [Fact]
    public async Task TestCase1()
    {
        using var pw = await Playwright.CreateAsync();
        await using var browser = await pw.Chromium.LaunchAsync();

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://en.ehu.lt/");

        var cookieButton = await page.QuerySelectorAsync(".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn");
        if (cookieButton != null)
        {
            await cookieButton.ClickAsync();
        }

        var footerAbout = page.Locator("//footer//a[contains(text(), 'About')]");
        await footerAbout.WaitForAsync();
        await footerAbout.ClickAsync();

        Assert.Equal("https://en.ehuniversity.lt/about/", page.Url);

        var title = await page.TitleAsync();
        Assert.Contains("About", title);
    }

    [Fact]
    public async Task TestCase2()
    {
        using var pw = await Playwright.CreateAsync();
        await using var browser = await pw.Chromium.LaunchAsync();

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://en.ehu.lt/");

        var cookieButton = await page.QuerySelectorAsync(".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn");
        if (cookieButton != null)
        {
            await cookieButton.ClickAsync();
        }
        await page.Locator("//div[@class='header-search']").ClickAsync();

        var searchInput = page.Locator("input[plerdy-tracking-id='30561103501']");

        await searchInput.FillAsync("study programs");

        await page.Locator("button[plerdy-tracking-id='66179574101']").ClickAsync();

        Assert.Contains("/?s=study+programs", page.Url);

    }

    [Fact]
    public async Task TestCase3()
    {
        using var pw = await Playwright.CreateAsync();
        await using var browser = await pw.Chromium.LaunchAsync();

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://en.ehu.lt/");

        var cookieButton = await page.QuerySelectorAsync(".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn");
        if (cookieButton != null)
        {
            await cookieButton.ClickAsync();
        }

        var languageChoose = page.Locator("//li[@plerdy-tracking-id='39047282601']");
        await page.Locator("li[plerdy-tracking-id='41154587901']").ClickAsync();
        await languageChoose.WaitForAsync();
        await languageChoose.ClickAsync();

        Assert.Equal(@"https://lt.ehuniversity.lt/", page.Url);
    }

    [Fact]
    public async Task TestCase4()
    {
        using var pw = await Playwright.CreateAsync();
        await using var browser = await pw.Chromium.LaunchAsync();

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://en.ehu.lt/contact");

        var cookieButton = await page.QuerySelectorAsync(".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn");
        if (cookieButton != null)
        {
            await cookieButton.ClickAsync();
        }

        var emailLocator = page.Locator("a[plerdy-tracking-id='35448735101']");
        Assert.True(await emailLocator.IsVisibleAsync());
        var emailText = await emailLocator.InnerTextAsync();
        Assert.Equal("franciskscarynacr@gmail.com", emailText);

        var phoneLtLocator = page.Locator("li[plerdy-tracking-id='50296369501']");
        Assert.True(await phoneLtLocator.IsVisibleAsync());
        var phoneLtText = await phoneLtLocator.InnerTextAsync();
        Assert.Contains("+370 68 771365", phoneLtText);

        var phoneByLocator = page.Locator("li[plerdy-tracking-id='39744896801']");
        Assert.True(await phoneByLocator.IsVisibleAsync());
        var phoneByText = await phoneByLocator.InnerTextAsync();
        Assert.Contains("+375 29 5781488", phoneByText);

        var socialNetworksLocator = page.Locator("li[plerdy-tracking-id='64965466401']");
        Assert.True(await socialNetworksLocator.IsVisibleAsync());
        var socialNetworksText = await socialNetworksLocator.InnerTextAsync();
        Assert.Equal("Join us in the social networks: Facebook Telegram VK", socialNetworksText);
    }
}