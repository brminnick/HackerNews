using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Amazon;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Amazon.Runtime;

namespace HackerNews
{
    static class TextAnalysisService
    {
        #region Constant Fields
        readonly static Lazy<AmazonComprehendClient> _comprehendClient = new Lazy<AmazonComprehendClient>(() =>
            new AmazonComprehendClient(new BasicAWSCredentials(TextAnalysisConstants.AccessKey, TextAnalysisConstants.SecretKey), RegionEndpoint.USWest2));

        #endregion

        #region Properties
        static AmazonComprehendClient ComprehendClient => _comprehendClient.Value;
        #endregion

        #region Methods
        public static async Task<SentimentType> GetSentiment(string text)
        {
            var sentimentDocument = new DetectSentimentRequest
            {
                Text = text,
                LanguageCode = LanguageCode.En
            };

            var sentimentResults = await ComprehendClient.DetectSentimentAsync(sentimentDocument).ConfigureAwait(false);

            return sentimentResults?.Sentiment;
        }

        public static async Task<Dictionary<string, SentimentType>> GetSentiment(List<string> textList)
        {
            Debug.WriteLine($"textList.Count: {textList.Count}");

            var resultsDictionary = new Dictionary<string, SentimentType>();

            var currentTextIndex = 0;
            while (currentTextIndex < textList.Count - 1)
            {
                Debug.WriteLine($"currentTextIndex: {currentTextIndex}");

                var multiLanguageBatchInput = new BatchDetectSentimentRequest { LanguageCode = LanguageCode.En };

                while (multiLanguageBatchInput.TextList.Count < 25 && currentTextIndex < textList.Count)
                {
                    multiLanguageBatchInput.TextList.Add(textList[currentTextIndex++]);
                }

                var sentimentResults = await ComprehendClient.BatchDetectSentimentAsync(multiLanguageBatchInput).ConfigureAwait(false);

                if (sentimentResults?.ErrorList?.Any() ?? false)
                {
                    var exceptionList = sentimentResults.ErrorList.Select(x => new Exception($"Error Code: {x.ErrorCode}, Message: {x.ErrorMessage}"));
                    throw new AggregateException(exceptionList);
                }

                foreach (var result in sentimentResults?.ResultList?.Where(x => x != null))
                {
                    var textListIndex = (int)Math.Floor((currentTextIndex / 25.0) - 1) * 25 + result.Index;
                    Debug.WriteLine($"\ttextListIndex: {textListIndex}");

                    var doesStoryExist = resultsDictionary.ContainsKey(textList[textListIndex]);
                    if (!doesStoryExist)
                        resultsDictionary.Add(textList[textListIndex], result.Sentiment);
                }

                multiLanguageBatchInput.TextList.Clear();
            }

            return resultsDictionary;
        }
        #endregion
    }
}
