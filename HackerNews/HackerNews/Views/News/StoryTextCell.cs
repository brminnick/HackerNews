using System;

using Xamarin.Forms;

using HackerNews.Shared;

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
                Text = story.Title;
                Detail = $"{story.TitleSentimentEmoji}{story.Score} Points by {story.Author}, {GetAgeOfStory(story.CreatedAt_DateTimeOffset)} ago";
            }
        }

        string GetAgeOfStory(DateTimeOffset storyCreatedAt)
        {
            var timespanSinceStoryCreated = DateTimeOffset.UtcNow - storyCreatedAt;

            switch (timespanSinceStoryCreated)
            {
                case TimeSpan storyAge when storyAge < TimeSpan.FromHours(1):
                    return $"{Math.Ceiling(timespanSinceStoryCreated.TotalMinutes)} minutes";

                case TimeSpan storyAge when storyAge >= TimeSpan.FromHours(1) && storyAge < TimeSpan.FromHours(2):
                    return $"{Math.Floor(timespanSinceStoryCreated.TotalHours)} hour";

                case TimeSpan storyAge when storyAge >= TimeSpan.FromHours(2) && storyAge < TimeSpan.FromHours(24):
                    return $"{Math.Floor(timespanSinceStoryCreated.TotalHours)} hours";

                case TimeSpan storyAge when storyAge >= TimeSpan.FromHours(24) && storyAge < TimeSpan.FromHours(48):
                    return $"{Math.Floor(timespanSinceStoryCreated.TotalDays)} day";

                case TimeSpan storyAge when storyAge >= TimeSpan.FromHours(48):
                    return $"{Math.Floor(timespanSinceStoryCreated.TotalDays)} days";

                default:
                    return string.Empty;
            }
        }
    }
}
