using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using BlazorBootstrap;

using AppBibleNew.Service;
namespace AppBibleNew
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddSingleton<IBibleService, BibleService>();
            builder.Services.AddSingleton<IPrayerService, PrayerService>();
            builder.Services.AddSingleton<IRosaryService, RosaryService>();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
