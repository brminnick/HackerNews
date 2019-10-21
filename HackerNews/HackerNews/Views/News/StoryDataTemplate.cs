using System;

using Xamarin.Forms;

using HackerNews.Shared;

namespace HackerNews
{
    class StoryDataTemplate : DataTemplate
    {
        public StoryDataTemplate(StoryModel story) : base(() => CreateStoryDataTemplate(story))
        {
        }

        static Grid CreateStoryDataTemplate(in StoryModel story)
        {
            var storyTitleLabel = new Label
            {
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                Text = story.Title
            };

            var storyDetailLabel = new Label
            {
                FontSize = 12,
                Text = $"{story.TitleSentimentEmoji}{story.Score} Points by {story.Author}, {GetAgeOfStory(story.CreatedAt_DateTimeOffset)} ago"
            };

            var grid = new Grid
            {
                Margin = new Thickness(5),
                RowSpacing = 2,

                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(15, GridUnitType.Absolute) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(storyTitleLabel, 0, 0);
            grid.Children.Add(storyDetailLabel, 0, 1);

            return grid;

            static string GetAgeOfStory(in DateTimeOffset storyCreatedAt)
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

    class StoryDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container) =>
            new StoryDataTemplate((StoryModel)item);
    }
}
