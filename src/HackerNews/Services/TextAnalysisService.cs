using Azure.AI.TextAnalytics;

namespace HackerNews;

class TextAnalysisService
{
	readonly TextAnalyticsClient _textAnalyticsApiClient;

	static bool _isApiKeyValid = true;

	public TextAnalysisService(TextAnalyticsClient textAnalyticsClient) => _textAnalyticsApiClient = textAnalyticsClient;

	public async Task<TextSentiment?> GetSentiment(string text)
	{
		if (!_isApiKeyValid)
			return null;

		try
		{
			var response = await _textAnalyticsApiClient.AnalyzeSentimentAsync(text).ConfigureAwait(false);
			return response.Value.Sentiment;
		}
		catch (Azure.RequestFailedException)
		{
			_isApiKeyValid = false;
			throw;
		}
		catch (AggregateException e) when (e.InnerExceptions.OfType<Azure.RequestFailedException>().Any())
		{
			_isApiKeyValid = false;
			throw;
		}
	}
}