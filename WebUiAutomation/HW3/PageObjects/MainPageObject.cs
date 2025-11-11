using Microsoft.Playwright;

namespace WebUiAutomation.HW3.PageObjects;

public class MainPageObject(IPage page) : BasePageObject(page)
{
    private readonly string _selectorLithuanianLanguage = "//li[@plerdy-tracking-id='39047282601']";
    private readonly string _selectorLanguageChange = "li[plerdy-tracking-id='41154587901']";
    private readonly string _searchField = "input[plerdy-tracking-id='30561103501']";
    private readonly string _submitButton = "button[plerdy-tracking-id='66179574101']";
    private readonly string _selectorAboutInFooter = "//footer//a[contains(text(), 'About')]";
    private readonly string _headerSearch = "//div[@class='header-search']";

    public async Task<MainPageObject> FillInputSearchProgramAsync(string name)
    {
        var searchInput = Page.Locator(_searchField);
        await searchInput.FillAsync(name);

        return this;
    }

    public async Task<MainPageObject> ClickSearchHeader()
    {
        await Page.Locator(_headerSearch).ClickAsync();
        return this;
    }

    public async Task<MainPageObject> ClickSearchButton()
    {
        await Page.Locator(_submitButton).ClickAsync();
        return this;
    }

    public async Task<MainPageObject> GoToAboutPageUsingFooter()
    {
        var footerAbout = Page.Locator(_selectorAboutInFooter);
        await footerAbout.WaitForAsync();
        await footerAbout.ClickAsync();
        return this;
    }

    public async Task<MainPageObject> GoToContactPage()
    {
        await Page.GotoAsync(TestSettings.ContactUrl);
        return this;
    }

    public async Task<MainPageObject> ClickOnChooseLanguage()
    {
        await Page.Locator(_selectorLanguageChange).ClickAsync();
        return this;
    }

    public async Task<MainPageObject> ClickOnLithuanianLanguage()
    {
        var languageChoose = Page.Locator(_selectorLithuanianLanguage);
        await languageChoose.WaitForAsync();
        await languageChoose.ClickAsync();
        return this;
    }
}