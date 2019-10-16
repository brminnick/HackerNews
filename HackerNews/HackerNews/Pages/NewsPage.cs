using System.Collections;
using HackerNews.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HackerNews
{
    public class NewsPage : BaseContentPage<NewsViewModel>
    {
        readonly ListView _storiesListView;

        public NewsPage() : base("Top Stories")
        {
            ViewModel.PullToRefreshFailed += HandlePullToRefreshFailed;

            _storiesListView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                RefreshControlColor = Color.Black,
                ItemTemplate = new DataTemplate(typeof(StoryTextCell)),
                IsPullToRefreshEnabled = true,
                BackgroundColor = Color.FromHex("F6F6EF"),
                SeparatorVisibility = SeparatorVisibility.None
            };
            _storiesListView.ItemTapped += HandleItemTapped;
            _storiesListView.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.TopStoryCollection));
            _storiesListView.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsListRefreshing));
            _storiesListView.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.RefreshCommand));

            Content = _storiesListView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_storiesListView.ItemsSource is ICollection collection && collection.Count <= 0)
            {
                _storiesListView.BeginRefresh();
            }
        }

        void HandleItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView listView && e?.Item is StoryModel storyTapped)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    listView.SelectedItem = null;

                    var browserOptions = new BrowserLaunchOptions
                    {
                        PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
                        PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
                    };

                    await Browser.OpenAsync(storyTapped.Url, browserOptions);
                });
            }
        }

        void HandlePullToRefreshFailed(object sender, string message) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Refresh Failed", message, "OK"));
    }
}
