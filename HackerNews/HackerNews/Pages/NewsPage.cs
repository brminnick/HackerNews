using System.Collections;
using HackerNews.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HackerNews
{
    public class NewsPage : BaseContentPage<NewsViewModel>
    {
        public NewsPage() : base("Top Stories")
        {
            ViewModel.PullToRefreshFailed += HandlePullToRefreshFailed;

            var storiesListView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                RefreshControlColor = Color.Black,
                ItemTemplate = new DataTemplate(typeof(StoryTextCell)),
                IsPullToRefreshEnabled = true,
                BackgroundColor = Color.FromHex("F6F6EF"),
                SeparatorVisibility = SeparatorVisibility.None
            };
            storiesListView.ItemTapped += HandleItemTapped;
            storiesListView.SetBinding(ListView.ItemsSourceProperty, nameof(NewsViewModel.TopStoryCollection));
            storiesListView.SetBinding(ListView.IsRefreshingProperty, nameof(NewsViewModel.IsListRefreshing));
            storiesListView.SetBinding(ListView.RefreshCommandProperty, nameof(NewsViewModel.RefreshCommand));

            Content = storiesListView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Content is ListView listView && IsNullOrEmpty(listView.ItemsSource))
            {
                listView.BeginRefresh();
            }

            static bool IsNullOrEmpty(in IEnumerable enumerable) => !enumerable?.GetEnumerator().MoveNext() ?? true;
        }

        async void HandleItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                var listView = (ListView)sender;
                listView.SelectedItem = null;

                switch (e?.Item)
                {
                    case StoryModel storyWithValidUrl when !string.IsNullOrEmpty(storyWithValidUrl.Url):
                        var browserOptions = new BrowserLaunchOptions
                        {
                            PreferredControlColor = ColorConstants.BrowserNavigationBarTextColor,
                            PreferredToolbarColor = ColorConstants.BrowserNavigationBarBackgroundColor
                        };

                        return Browser.OpenAsync(storyWithValidUrl.Url, browserOptions);

                    default:
                        return DisplayAlert("Invalid Article", "ASK HN articles have no url", "OK");
                }
            });
        }

        void HandlePullToRefreshFailed(object sender, string message) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Refresh Failed", message, "OK"));
    }
}
