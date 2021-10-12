using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Refit;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HackerNews
{
    public class Startup
    {
        public static MauiApp Create()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            // Services
            builder.Services.AddSingleton(RestService.For<IHackerNewsAPI>("https://hacker-news.firebaseio.com/v0"));
            builder.Services.AddSingleton<HackerNewsAPIService>();

            // View Models
            builder.Services.AddTransient<NewsViewModel>();

            // Pages
            builder.Services.AddTransient<NewsPage>();

            return builder.Build();
        }
    }
}