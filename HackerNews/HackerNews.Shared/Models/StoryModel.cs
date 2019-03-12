using System;

using Amazon.Comprehend;

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
        public string TitleSentimentEmoji => GetEmoji(TitleSentimentScore);

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

        public SentimentType TitleSentimentScore { get; set; } = new SentimentType(null);

        DateTimeOffset UnixTimeStampToDateTimeOffset(long unixTimeStamp)
        {
            var dateTimeOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, default);
            return dateTimeOffset.AddSeconds(unixTimeStamp);
        }

        string GetEmoji(SentimentType sentimentType)
        {
            if (sentimentType.Equals(SentimentType.NEUTRAL) || sentimentType.Equals(SentimentType.MIXED))
                return EmojiConstants.NeutralFaceEmoji;

            if (sentimentType.Equals(SentimentType.NEGATIVE))
                return EmojiConstants.SadFaceEmoji;

            if (sentimentType.Equals(SentimentType.POSITIVE))
                return EmojiConstants.HappyFaceEmoji;

            if (sentimentType.Equals(null))
                return EmojiConstants.BlankFaceEmoji;

            return string.Empty;
        }
    }
}
