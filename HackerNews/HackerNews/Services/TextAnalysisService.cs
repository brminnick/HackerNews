using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;

namespace HackerNews
{
    static class TextAnalysisService
    {
        readonly static Lazy<TextAnalyticsClient> _textAnalyticsApiClientHolder = new Lazy<TextAnalyticsClient>(() =>
            new TextAnalyticsClient(new ApiKeyServiceClientCredentials(TextAnalysisConstants.SentimentKey)) { Endpoint = TextAnalysisConstants.BaseUrl });

        static TextAnalyticsClient TextAnalyticsApiClient => _textAnalyticsApiClientHolder.Value;

        public static async Task<double?> GetSentiment(string text)
        {
            var sentimentResult = await TextAnalyticsApiClient.SentimentAsync(text).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(sentimentResult?.ErrorMessage))
                throw new Exception(sentimentResult?.ErrorMessage);

            return sentimentResult?.Score;
        }

        public static async Task<Dictionary<string, double?>> GetSentiment(IEnumerable<string> textList)
        {
            var textIdDictionary = new Dictionary<string, string>();
            var multiLanguageBatchInput = new MultiLanguageBatchInput(Enumerable.Empty<MultiLanguageInput>().ToList());

            foreach (var text in textList)
            {
                var textGuidString = Guid.NewGuid().ToString();

                textIdDictionary.Add(textGuidString, text);

                multiLanguageBatchInput.Documents.Add(new MultiLanguageInput(textGuidString, text));
            }

            var sentimentResults = await TextAnalyticsApiClient.SentimentBatchAsync(multiLanguageBatchInput).ConfigureAwait(false);

            if (sentimentResults?.Errors?.Any() is true)
            {
                var exceptionList = sentimentResults.Errors.Select(x => new Exception($"Id: {x.Id}, Message: {x.Message}"));
                throw new AggregateException(exceptionList);
            }

            var resultsDictionary = new Dictionary<string, double?>();

            foreach (var result in sentimentResults?.Documents?.Where(x => x != null))
            {
                var doesStoryExist = resultsDictionary.ContainsKey(textIdDictionary[result.Id]);

                if (!doesStoryExist)
                    resultsDictionary.Add(textIdDictionary[result.Id], result.Score);
            }

            return resultsDictionary;
        }
    }

    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        readonly string _subscriptionKey;

        public ApiKeyServiceClientCredentials(string subscriptionKey) => _subscriptionKey = subscriptionKey;

        public override Task ProcessHttpRequestAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            return Task.CompletedTask;
        }
    }
}
