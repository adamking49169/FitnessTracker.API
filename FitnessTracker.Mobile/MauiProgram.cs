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

        builder.Services.AddHttpClient<ApiService>(client =>
        {
#if ANDROID
            // When running on an Android emulator, use the host machine
            // address `10.0.2.2` to reach the local API running under IIS
            // Express on https://localhost:44363.
            client.BaseAddress = new Uri("https://10.0.2.2:44363/");
#else
            client.BaseAddress = new Uri("https://localhost:44363/");
#endif
        });

        return builder.Build();
    }
}
