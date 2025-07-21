using FitnessTracker.Mobile.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Read the base API URL from an environment variable when available.
        var baseUrl = builder.Configuration["API_BASE_URL"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
#if ANDROID
            // Use the host loopback address to reach IIS Express when
            // running on an Android emulator.
            baseUrl = "https://10.0.2.2:44363/";
#else
            baseUrl = "https://localhost:44363/";
#endif
        }

        builder.Services.AddHttpClient<ApiService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });

        return builder.Build();
    }
}
