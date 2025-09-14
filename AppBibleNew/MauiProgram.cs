using AppBibleNew.Service;
using BlazorBootstrap;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using AppBibleNew.Service;
using Plugin.Maui.Audio;
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
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "notes.db");
            builder.Services.AddSingleton(new NoteService(dbPath));
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddSingleton<IBibleService, BibleService>();
            builder.Services.AddSingleton<IPrayerService, PrayerService>();
            builder.Services.AddSingleton<IRosaryService, RosaryService>();
            builder.Services.AddSingleton<AudioService>();

          

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
