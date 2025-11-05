using Microsoft.Playwright;

namespace WebUiAutomation.HW3;

public class SearchProgramPage(IPage page)
{
    private const string _searchField = "input[plerdy-tracking-id='30561103501']";
    private const string _submitButton = "button[plerdy-tracking-id='66179574101']";
    private readonly string _headerSearch = "//div[@class='header-search']";

    public async Task<SearchProgramPage> SearchProgramAsync(string name)
    {
        await page.Locator(_headerSearch).ClickAsync();
        var searchInput = page.Locator(_searchField);
        await searchInput.FillAsync(name);
        await page.Locator(_submitButton).ClickAsync();
        return this;
    }
}