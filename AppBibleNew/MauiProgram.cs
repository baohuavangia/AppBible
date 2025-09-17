using AppBibleNew.Service;
using BlazorBootstrap;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using AppBibleNew.Services;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI.Windowing;
using System.Runtime.InteropServices;

namespace AppBibleNew
{
    public static class MauiProgram
    {
#if WINDOWS
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_MAXIMIZE = 3;
#endif

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
            builder.Services.AddSingleton<PomodoroService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // ⚡ Mở cửa sổ full màn hình khi chạy trên Windows
            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        ShowWindow(hwnd, SW_MAXIMIZE);
                    });
                });
#endif
            });

            return builder.Build();
        }
    }
}
