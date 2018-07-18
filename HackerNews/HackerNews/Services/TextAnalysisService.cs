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
        #region Constant Fields
        readonly static Lazy<TextAnalyticsClient> _textAnalyticsApiClientHolder = new Lazy<TextAnalyticsClient>(() =>
            new TextAnalyticsClient(new ApiKeyServiceClientCredentials(TextAnalysisConstants.SentimentKey)) { BaseUri = new Uri("https://westus.api.cognitive.microsoft.com/text/analytics/v2.0") });

        #endregion

        #region Properties
        static TextAnalyticsClient TextAnalyticsApiClient => _textAnalyticsApiClientHolder.Value;
        #endregion

        #region Methods
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

        public static async Task<Dictionary<string, double?>> GetSentiment(List<string> textList)
        {
            var textIdDictionary = new Dictionary<string, string>();
            var multiLanguageBatchInput = new MultiLanguageBatchInput(new List<MultiLanguageInput>());

            foreach (var text in textList)
            {
                var textGuidString = Guid.NewGuid().ToString();

                textIdDictionary.Add(textGuidString, text);

                multiLanguageBatchInput.Documents.Add(new MultiLanguageInput(id: textGuidString, text: text));
            }

            var sentimentResults = await TextAnalyticsApiClient.SentimentAsync(multiLanguageBatchInput).ConfigureAwait(false);

            if (sentimentResults?.Errors?.Any() ?? false)
            {
                var exceptionList = sentimentResults.Errors.Select(x => new Exception($"Id: {x.Id}, Message: {x.Message}"));
                throw new AggregateException(exceptionList);
            }

            var resultsDictionary = new Dictionary<string, double?>();

            foreach (var result in sentimentResults?.Documents)
                resultsDictionary.Add(textIdDictionary[result.Id], result?.Score);

            return resultsDictionary;
        }
        #endregion
    }

    #region Classes
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
    #endregion
}
