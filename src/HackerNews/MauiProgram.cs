using System;
using Azure;
using Azure.AI.TextAnalytics;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;
using Polly;
using Refit;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HackerNews;

public class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().UseMauiCommunityToolkitMarkup();

		// App
		builder.Services.AddSingleton<App>();
		builder.Services.AddSingleton<AppShell>();

		// Services
		builder.Services.AddSingleton<TextAnalysisService>();
		builder.Services.AddSingleton<HackerNewsAPIService>();
		builder.Services.AddSingleton(new TextAnalyticsClient(new Uri(TextAnalysisConstants.BaseUrl), new AzureKeyCredential(TextAnalysisConstants.SentimentKey)));
		builder.Services.AddRefitClient<IHackerNewsAPI>()
							.ConfigureHttpClient(client => client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0"))
							.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(3, sleepDurationProvider));

		builder.Services.AddSingleton(Browser.Default);

		// View Models
		builder.Services.AddTransient<NewsViewModel>();

		// Pages
		builder.Services.AddTransient<NewsPage>();

		return builder.Build();

		static TimeSpan sleepDurationProvider(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
	}
}