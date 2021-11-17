using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;
using Refit;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HackerNews
{
    public class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            // Services
            builder.Services.AddSingleton<App>();
            builder.Services.AddSingleton<TextAnalysisService>();
            builder.Services.AddSingleton<HackerNewsAPIService>();
            builder.Services.AddSingleton(RestService.For<IHackerNewsAPI>("https://hacker-news.firebaseio.com/v0"));
            builder.Services.AddSingleton(new TextAnalyticsClient(new Uri(TextAnalysisConstants.BaseUrl), new AzureKeyCredential(TextAnalysisConstants.SentimentKey)));

            // View Models
            builder.Services.AddTransient<NewsViewModel>();

            // Pages
            builder.Services.AddTransient<NewsPage>();

            return builder.Build();
        }
    }
}