using Newtonsoft.Json;

namespace HackerNews
{
    public class NaturalLanguageResponseModel
    {
        [JsonProperty("documentSentiment")]
        public Sentiment DocumentSentiment { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("sentences")]
        public Sentence[] Sentences { get; set; }

    }

    public class Sentiment
    {
        [JsonProperty("magnitude")]
        public double Magnitude { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class Sentence
    {
        [JsonProperty("text")]
        public Text Text { get; set; }

        [JsonProperty("sentiment")]
        public Sentiment Sentiment { get; set; }
    }

    public class Text
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("beginOffset")]
        public long BeginOffset { get; set; }
    }
}
