using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace WebUiAutomation.HW3;

public static class TestSettings
{
    private static readonly IConfigurationRoot Configuration;

    static TestSettings()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile(@"C:\Users\lukas\source\repos\AQA\WebUiAutomation\appsettings.json")
            .Build();
    }

    public static string MainUrl => Configuration["MainUrl"] ?? string.Empty;

    public static string ContactUrl => Configuration["ContactUrl"] ?? string.Empty;
}
