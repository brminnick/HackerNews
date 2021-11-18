using System.Threading.Tasks;
using Azure.AI.TextAnalytics;

namespace HackerNews;

class TextAnalysisService
{
	readonly TextAnalyticsClient _textAnalyticsApiClient;

	public TextAnalysisService(TextAnalyticsClient textAnalyticsClient) => _textAnalyticsApiClient = textAnalyticsClient;

	public async Task<TextSentiment> GetSentiment(string text)
	{
		var response = await _textAnalyticsApiClient.AnalyzeSentimentAsync(text).ConfigureAwait(false);
		return response.Value.Sentiment;
	}
}
