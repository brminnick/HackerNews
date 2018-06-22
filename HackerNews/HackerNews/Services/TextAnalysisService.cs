using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HackerNews
{
    abstract class TextAnalysisService : BaseHttpClientService
    {
        #region Methods
        public static async Task<double?> GetSentiment(string text)
        {
            var result = await GetSentimentFromAPI(text).ConfigureAwait(false);
            return result.sentiment;
        }

        public static async Task<Dictionary<string, double?>> GetSentiment(List<string> textList)
        {
            var resultsTaskList = new List<Task<(string text, double? sentiment)>>();
            resultsTaskList.AddRange(textList.Select(x => GetSentimentFromAPI(x)));

            await Task.WhenAll(resultsTaskList);

            var sentimentDictionary = new Dictionary<string, double?>();

            foreach (var resultTask in resultsTaskList)
            {
                var (text, sentiment) = await resultTask.ConfigureAwait(false);
                sentimentDictionary.Add(text, sentiment);
            }

            return sentimentDictionary;
        }

        static async Task<(string text, double? sentiment)> GetSentimentFromAPI(string text)
        {
            var request = new NaturalLanguageRequestModel(new Document(text));

            var response = await PostObjectToAPI<NaturalLanguageResponseModel, NaturalLanguageRequestModel>($"https://language.googleapis.com/v1/documents:analyzeSentiment?key={NaturalLanguageConstants.ApiKey}", request).ConfigureAwait(false);

            return (text, response?.DocumentSentiment?.Score);
        }
        #endregion
    }
}
