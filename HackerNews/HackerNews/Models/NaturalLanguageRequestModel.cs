using Newtonsoft.Json;

namespace HackerNews
{
    public class NaturalLanguageRequestModel
    {
        public NaturalLanguageRequestModel(Document document) => Document = document;

        [JsonProperty("document")]
        public Document Document { get; }
    }

    public class Document
    {
        const string _plainText = "PLAIN_TEXT";

        public Document(string content) => Content = content;

        [JsonProperty("type")]
        public string Type => _plainText;

        [JsonProperty("content")]
        public string Content { get; }
    }
}
