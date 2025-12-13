using Microsoft.Playwright;
using NUnit.Framework;
using Reqnroll;
using WebUiAutomation.HW3;
using WebUiAutomation.HW3.PageObjects;



namespace WebUiAutomation.HW6
{
    [Binding]
    public class HW6
    {
        private const string CookieButtonsSelector = ".cc-btn.cc-dismiss, .cc-btn.cc-allow, .cc-btn";
        private const string LithuanianSite = "https://lt.ehuniversity.lt/";
        private const string StudyProgramPath = "/?s=study+programs";
        private const string AboutUrl = "https://en.ehuniversity.lt/about/";

        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;

        private MainPageObject _mainPage;
        private AboutPageObject _aboutPage;
        private ContactPageObject _contactPage;

        [BeforeScenario]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync();
            _context = await _browser.NewContextAsync();

            var page = await _context.NewPageAsync();
            await page.GotoAsync(TestSettings.MainUrl);

            _mainPage = new MainPageObject(page);
        }

        [AfterScenario]
        public async Task Cleanup()
        {
            await _mainPage.ClosePage();
            await _context.DisposeAsync();
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }


        [Given(@"I open the EHU main page")]
        public void GivenIOpenMainPage()
        {
            Assert.IsTrue(_mainPage.GetPageUrl().Contains("ehuniversity"));
        }

        [Given(@"I accept cookies if they appear")]
        public async Task AcceptCookies()
        {
            var button = await _mainPage.GetPage().QuerySelectorAsync(CookieButtonsSelector);
            if (button != null)
            {
                await button.ClickAsync();
            }
        }

        [When(@"I open the language selector")]
        public async Task OpenLanguageSelector()
        {
            await _mainPage.ClickOnChooseLanguage();
        }

        [When(@"I select Lithuanian language")]
        public async Task SelectLithuanian()
        {
            await _mainPage.ClickOnLithuanianLanguage();
        }

        [Then(@"the Lithuanian version of the website should be opened")]
        public void VerifyLithuanianSite()
        {
            StringAssert.Contains(LithuanianSite, _mainPage.GetPageUrl());
        }

        [When(@"I open the search panel")]
        public async Task OpenSearch()
        {
            await _mainPage.ClickSearchHeader();
        }

        [When(@"I search for ""([^""]*)""")]
        public async Task SearchProgram(string text)
        {
            await _mainPage.FillInputSearchProgramAsync(text);
            await _mainPage.ClickSearchButton();
        }

        [Then(@"search results page should display study programs")]
        public void VerifySearchResults()
        {
            StringAssert.Contains(StudyProgramPath, _mainPage.GetPageUrl());
        }

        [When(@"I navigate to About page from the footer")]
        public async Task NavigateToAbout()
        {
            await _mainPage.GoToAboutPageUsingFooter();
            _aboutPage = new AboutPageObject(_mainPage.GetPage());
        }

        [Then(@"About page should be opened")]
        public void VerifyAboutUrl()
        {
            Assert.AreEqual(AboutUrl, _aboutPage.GetPageUrl());
        }

        [Then(@"the about page title should contain ""([^""]*)""")]
        public async Task VerifyAboutTitle(string text)
        {
            var title = await _aboutPage.GetTitleAboutAsync();
            StringAssert.Contains(text, title);
        }

        [When(@"I open the Contact page")]
        public async Task OpenContact()
        {
            await _mainPage.GoToContactPage();
            _contactPage = new ContactPageObject(_mainPage.GetPage());
        }

        [Then(@"contact element ""([^""]*)"" should contain ""([^""]*)""")]
        public async Task VerifyContactInfo(string selector, string expectedText)
        {
            await _contactPage.WaitUntilContactInfoVisiable(selector);
            var actual = await _contactPage.GetTextBySelector(selector);

            StringAssert.Contains(expectedText, actual);
        }
    }
}
