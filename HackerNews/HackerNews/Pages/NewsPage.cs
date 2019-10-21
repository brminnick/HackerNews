using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HackerNews.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HackerNews
{
    public class NewsPage : BaseContentPage<NewsViewModel>
    {
        readonly RefreshView _storiesCollectionRefreshView;

        public NewsPage() : base("Top Stories")
        {
            ViewModel.PullToRefreshFailed += HandlePullToRefreshFailed;

            var backgroundColor = Color.FromHex("F6F6EF");

            var storiesCollectionView = new CollectionView
            {
                BackgroundColor = backgroundColor,
                ItemTemplate = new StoryDataTemplateSelector(),
                SelectionMode = SelectionMode.Single
            };
            storiesCollectionView.SelectionChanged += HandleStoriesCollectionSelectionChanged;
            storiesCollectionView.SetBinding(CollectionView.ItemsSourceProperty, nameof(NewsViewModel.TopStoryCollection));

            _storiesCollectionRefreshView = new RefreshView
            {
                BackgroundColor = backgroundColor,
                RefreshColor = Color.Black,
                Content = storiesCollectionView
            };
            _storiesCollectionRefreshView.SetBinding(RefreshView.IsRefreshingProperty, nameof(NewsViewModel.IsListRefreshing));
            _storiesCollectionRefreshView.SetBinding(RefreshView.CommandProperty, nameof(NewsViewModel.RefreshCommand));

            Content = _storiesCollectionRefreshView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_storiesCollectionRefreshView.Content is CollectionView collectionView
                && collectionView.ItemsSource is ICollection<StoryModel> storiesCollection
                    && !storiesCollection.Any())
            {
                _storiesCollectionRefreshView.IsRefreshing = true;
            }
        }

        async void HandleStoriesCollectionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collectionView = (CollectionView)sender;

            if (e?.CurrentSelection.FirstOrDefault() is StoryModel storyTapped)
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    collectionView.SelectedItem = null;

                    var browserOptions = new BrowserLaunchOptions
                    {
                        PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
                        PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
                    };

                    return Browser.OpenAsync(storyTapped.Url, browserOptions);
                });
            }
        }

        void HandlePullToRefreshFailed(object sender, string message) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Refresh Failed", message, "OK"));
    }
}
