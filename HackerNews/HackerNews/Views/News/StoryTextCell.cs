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

            var story = (StoryModel)BindingContext;

            Text = story.Title;
            Detail = $"{story.TitleSentimentEmoji} {story.Score} Points by {story.Author}, {GetAgeOfStory(story.CreatedAt_DateTimeOffset)} ago";
        }

        string GetAgeOfStory(DateTimeOffset storyCreatedAt)
        {
            var timespanSinceStoryCreated = DateTimeOffset.UtcNow - storyCreatedAt;

            return timespanSinceStoryCreated switch
            {
                TimeSpan storyAge when storyAge < TimeSpan.FromHours(1) => $"{Math.Ceiling(timespanSinceStoryCreated.TotalMinutes)} minutes",

                TimeSpan storyAge when storyAge >= TimeSpan.FromHours(1) && storyAge < TimeSpan.FromHours(2) => $"{Math.Floor(timespanSinceStoryCreated.TotalHours)} hour",

                TimeSpan storyAge when storyAge >= TimeSpan.FromHours(2) && storyAge < TimeSpan.FromHours(24) => $"{Math.Floor(timespanSinceStoryCreated.TotalHours)} hours",

                TimeSpan storyAge when storyAge >= TimeSpan.FromHours(24) && storyAge < TimeSpan.FromHours(48) => $"{Math.Floor(timespanSinceStoryCreated.TotalDays)} day",

                TimeSpan storyAge when storyAge >= TimeSpan.FromHours(48) => $"{Math.Floor(timespanSinceStoryCreated.TotalDays)} days",

                _ => string.Empty,
            };
        }
    }
}
