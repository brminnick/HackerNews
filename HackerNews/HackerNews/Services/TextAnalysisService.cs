using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace HackerNews
{
	static class TextAnalysisService
	{
		readonly static Lazy<TextAnalyticsAPI> _textAnalyticsApiClientHolder = new Lazy<TextAnalyticsAPI>(() =>
			new TextAnalyticsAPI
			{
				AzureRegion = AzureRegions.Westus,
				SubscriptionKey = TextAnalysisConstants.SentimentKey
			});

		static TextAnalyticsAPI TextAnalyticsApiClient => _textAnalyticsApiClientHolder.Value;

		public static async Task<double?> GetSentiment(string text)
		{
			var sentimentDocument = new MultiLanguageBatchInput(new List<MultiLanguageInput> { { new MultiLanguageInput(id: "1", text: text) } });

			var sentimentResults = await TextAnalyticsApiClient.SentimentAsync(sentimentDocument).ConfigureAwait(false);

			if (sentimentResults?.Errors?.Any() ?? false)
			{
				var exceptionList = sentimentResults.Errors.Select(x => new Exception($"Id: {x.Id}, Message: {x.Message}"));
				throw new AggregateException(exceptionList);
			}

			var documentResult = sentimentResults?.Documents?.FirstOrDefault();

			return documentResult?.Score;
		}
	}
}
