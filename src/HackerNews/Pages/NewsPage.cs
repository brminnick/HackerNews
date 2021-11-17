using System.Collections;
using System.Linq;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using Microsoft.Maui.Essentials;

namespace HackerNews
{
    class NewsPage : BaseContentPage<NewsViewModel>
    {
        public NewsPage(NewsViewModel newsViewModel) : base(newsViewModel, "Top Stories")
        {
            ViewModel.PullToRefreshFailed += HandlePullToRefreshFailed;

            Content = new RefreshView
            {
                RefreshColor = Colors.Black,

                Content = new CollectionView
                {
                    BackgroundColor = Color.FromArgb("F6F6EF"),
                    SelectionMode = SelectionMode.Single,
                    ItemTemplate = new StoryDataTemplate(),

                }.Bind(CollectionView.ItemsSourceProperty, nameof(NewsViewModel.TopStoryCollection))
                 .Invoke(collectionView => collectionView.SelectionChanged += HandleSelectionChanged)

            }.Bind(RefreshView.IsRefreshingProperty, nameof(NewsViewModel.IsListRefreshing))
             .Bind(RefreshView.CommandProperty, nameof(NewsViewModel.RefreshCommand));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Content is RefreshView refreshView
                && refreshView.Content is CollectionView collectionView
                && IsNullOrEmpty(collectionView.ItemsSource))
            {
                refreshView.IsRefreshing = true;
            }

            static bool IsNullOrEmpty(in IEnumerable? enumerable) => !enumerable?.GetEnumerator().MoveNext() ?? true;
        }

        async void HandleSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var collectionView = (CollectionView)(sender ?? throw new NullReferenceException());
            collectionView.SelectedItem = null;

            if (e.CurrentSelection.FirstOrDefault() is StoryModel storyModel)
            {
                if (!string.IsNullOrEmpty(storyModel.Url))
                {
                    var browserOptions = new BrowserLaunchOptions
                    {
                        PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
                        PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
                    };

                    await Browser.OpenAsync(storyModel.Url, browserOptions);
                }
                else
                {
                    await DisplayAlert("Invalid Article", "ASK HN articles have no url", "OK");
                }
            }
        }

        void HandlePullToRefreshFailed(object? sender, string message) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Refresh Failed", message, "OK"));
    }
}
