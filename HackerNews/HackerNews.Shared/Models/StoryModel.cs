using System;
using Azure.AI.TextAnalytics;
using Newtonsoft.Json;

namespace HackerNews.Shared
{
    public class StoryModel
    {
        public StoryModel(long id, string author, long score, long createdAt_UnixTime, string title, string url)
        {
            Id = id;
            Author = author;
            Score = score;
            CreatedAt_UnixTime = createdAt_UnixTime;
            Title = title;
            Url = url;
        }

        public DateTimeOffset CreatedAt_DateTimeOffset => UnixTimeStampToDateTimeOffset(CreatedAt_UnixTime);

        public string TitleSentimentEmoji => TitleSentiment switch
        {
            TextSentiment.Negative => EmojiConstants.SadFaceEmoji,
            TextSentiment.Neutral => EmojiConstants.NeutralFaceEmoji,
            TextSentiment.Mixed => EmojiConstants.NeutralFaceEmoji,
            TextSentiment.Positive => EmojiConstants.HappyFaceEmoji,
            null => string.Empty,
            _ => throw new NotSupportedException()
        };

        [JsonProperty("id")]
        public long Id { get; }

        [JsonProperty("by")]
        public string Author { get; }

        [JsonProperty("score")]
        public long Score { get; }

        [JsonProperty("time")]
        public long CreatedAt_UnixTime { get; }

        [JsonProperty("title")]
        public string Title { get; }

        [JsonProperty("url")]
        public string Url { get; }

        public TextSentiment? TitleSentiment { get; set; }

        DateTimeOffset UnixTimeStampToDateTimeOffset(long unixTimeStamp)
        {
            var dateTimeOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, default);
            return dateTimeOffset.AddSeconds(unixTimeStamp);
        }
    }
}
