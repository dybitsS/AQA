using Microsoft.Playwright;

namespace WebUiAutomation.HW3.PageObjects;
public class BasePageObject
{
    protected readonly IPage Page;

    public BasePageObject(IPage page)
    {
        Page = page;
    }

    public async Task<string> GetTitleAboutAsync()
    {
        return await Page.TitleAsync();
    }

    public string GetPageUrl()
    {
        return Page.Url;
    }

    public async Task ClosePage()
    {
        await Page.CloseAsync();
    }

    public IPage GetPage()
    {
        return Page;
    }

    public async Task<string> GetTextBySelector(string selector)
    {
        return await Page.Locator(selector).InnerTextAsync();
    }
}

