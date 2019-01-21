using System;

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

        public double? TitleSentimentScore { get; set; } = -1;

        DateTimeOffset UnixTimeStampToDateTimeOffset(long unixTimeStamp)
        {
            var dateTimeOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, default);
            return dateTimeOffset.AddSeconds(unixTimeStamp);
        }

        string GetEmoji(double? sentimentScore)
        {
            switch (sentimentScore)
            {
                case double number when (number >= 0 && number < 0.4):
                    return EmojiConstants.SadFaceEmoji;
                case double number when (number >= 0.4 && number <= 0.6):
                    return EmojiConstants.NeutralFaceEmoji;
                case double number when (number > 0.6):
                    return EmojiConstants.HappyFaceEmoji;
                case null:
                    return EmojiConstants.BlankFaceEmoji;
                default:
                    return string.Empty;
            }
        }
    }
}
