using Microsoft.Playwright;
using System.Threading.Tasks;

namespace WebUiAutomation.HW3;
public class BrowerSinglton
{

    private static IBrowser _browser;

    private BrowerSinglton()
    {
    }

    public async static Task<IBrowser> GetBrowser(IPlaywright playwright)
    {
        if (_browser is null)
        {
            _browser = await playwright.Chromium.LaunchAsync();
        }

        return _browser;
    }

    public static async Task CloseBrowser()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync(); 
        }
    }
}
