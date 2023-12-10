using Microsoft.Extensions.Logging;
using RssMob.Views;

namespace RssMob;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<EmployeeDatabase>();
       // builder.Services.AddSingleton<Dashboard>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<Services.InspectionServices>();
        builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
		.UseMauiMaps();
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
