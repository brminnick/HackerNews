using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.Sentiment;

namespace HackerNews
{
    abstract class TextAnalysisService : BaseHttpClientService
    {
        #region Constant Fields
        readonly static Lazy<SentimentClient> sentimentClientHolder =
            new Lazy<SentimentClient>(() => new SentimentClient(TextAnalysisConstants.SentimentKey));
        #endregion

        #region Properties
        static SentimentClient SentimentClient => sentimentClientHolder.Value;
        #endregion

        #region Methods
        public static async Task<float?> GetSentiment(string text)
        {
            var sentimentDocument = new SentimentDocument { Id = "1", Text = text };
            var sentimentRequest = new SentimentRequest { Documents = new List<IDocument> { { sentimentDocument } } };

            UpdateActivityIndicatorStatus(true);

            var sentimentResults = await SentimentClient.GetSentimentAsync(sentimentRequest);

            UpdateActivityIndicatorStatus(false);

            var documentResult = sentimentResults.Documents.FirstOrDefault();

            return documentResult?.Score;
        }
        #endregion
    }
}
