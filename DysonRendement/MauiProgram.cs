using DysonRendement.Services;
using Microsoft.Extensions.Logging;

namespace DysonRendement;

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
        builder.Services.AddSingleton<IGps, Gps>();
        builder.Services.AddSingleton<ISensor, Sensor>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}