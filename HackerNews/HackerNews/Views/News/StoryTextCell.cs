using System;

using Xamarin.Forms;

namespace HackerNews
{
    public class StoryTextCell : TextCell
    {
        public StoryTextCell()
        {
            TextColor = ColorConstants.TextCellTextColor;
            DetailColor = ColorConstants.TextCellDetailColor;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is StoryModel story)
            {
                Text = story?.Title;
                Detail = $"{GetEmoji(story.TitleSentimentScore)}{story.Score} Points by {story.Author} {GetAgeOfStory(story.CreatedAt_DateTimeOffset)} ago";
            }
        }

        string GetEmoji(float? sentimentScore)
        {
            switch (sentimentScore)
            {
                case float number when (number < 0.4):
                    return EmojiConstants.SadFaceEmoji;
                case float number when (number >= 0.4 && number <= 0.6):
                    return EmojiConstants.NeutralFaceEmoji;
                case float number when (number > 0.6):
                    return EmojiConstants.HappyFaceEmoji;
                default:
                    return string.Empty;
            }
        }

        string GetAgeOfStory(DateTimeOffset storyCreatedAt)
        {
            var timespanSinceStoryCreated = DateTimeOffset.UtcNow - storyCreatedAt;

            if (timespanSinceStoryCreated < TimeSpan.FromHours(1))
                return $"{Math.Ceiling(timespanSinceStoryCreated.TotalMinutes)} minutes";

            if (timespanSinceStoryCreated >= TimeSpan.FromHours(1) && timespanSinceStoryCreated < TimeSpan.FromHours(2))
                return $"{Math.Floor(timespanSinceStoryCreated.TotalHours)} hour";

            return $"{Math.Floor(timespanSinceStoryCreated.TotalHours)} hours";
        }
    }
}
