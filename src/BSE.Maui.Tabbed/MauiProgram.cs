using BSE.Maui.Tabbed.Handlers;
using BSE.Tunes.Maui.Client.Controls;
using Microsoft.Extensions.Logging;

namespace BSE.Maui.Tabbed
{
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
                })
                .ConfigureMauiHandlers((handlers)=>
                {
#if ANDROID
                    handlers.AddHandler<TabbedPageContainer, TabbedContainerViewHandler>();
#endif
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
