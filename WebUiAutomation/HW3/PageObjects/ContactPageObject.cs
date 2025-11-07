using Microsoft.Playwright;

namespace WebUiAutomation.HW3.PageObjects;

public class ContactPageObject(IPage page) : BasePageObject(page)
{
    public async Task<ContactPageObject> WaitUntilContactInfoVisiable(string selector)
    {
        await page.Locator(selector).IsVisibleAsync();
        return this;
    }
}