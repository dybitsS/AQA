using NUnit.Framework;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
namespace WebUiAutomation.HW5;

public static class ExtentManager
{
    private static ExtentReports _extent;
    private static ExtentSparkReporter _reporter;

    public static ExtentReports GetExtent()
    {
        if (_extent == null)
        {
            var reportPath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "ExtentReport.html"
            );

            _reporter = new ExtentSparkReporter(reportPath);
            _reporter.Config.DocumentTitle = "Playwright Automation Report";
            _reporter.Config.ReportName = "UI Automation Results";
            _reporter.Config.TimeStampFormat = "yyyy-MM-dd HH:mm:ss";
            _extent = new ExtentReports();
            _extent.AddSystemInfo("Executed On", DateTime.Now.ToString());
            _extent.AttachReporter(_reporter);
        }

        return _extent;
    }
}
