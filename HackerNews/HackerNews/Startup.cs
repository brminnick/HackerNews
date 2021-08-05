using System;
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;
using Refit;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace HackerNews
{
    public class Startup : IStartup
	{
		public void Configure(IAppHostBuilder appBuilder)
		{
			appBuilder.UseMauiApp<App>()
				.ConfigureServices(services =>
				{
					// Services
					services.AddSingleton(new TextAnalyticsClient(new Uri(TextAnalysisConstants.BaseUrl), new AzureKeyCredential(TextAnalysisConstants.SentimentKey)));
					services.AddSingleton(RestService.For<IHackerNewsAPI>("https://hacker-news.firebaseio.com/v0"));
					services.AddSingleton<TextAnalysisService>();
					services.AddSingleton<HackerNewsAPIService>();

					// View Models
					services.AddTransient<NewsViewModel>();

					// Pages
					services.AddTransient<NewsPage>();
				});
		}
	}
}