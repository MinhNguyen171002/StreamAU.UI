using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using UserUI.Data.Service;
using CommunityToolkit.Maui;
using Plugin.Maui.Audio;

namespace UserUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddTransient<HttpClient>();
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddBlazoredLocalStorage();            
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();


#if DEBUG
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();            

#endif
            builder.Services.AddTransient<Service>();
            builder.Services.AddTransient<SpeechtoText>();
            builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();
            return builder.Build();
        }
    }
}