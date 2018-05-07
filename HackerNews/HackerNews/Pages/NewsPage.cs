using System;

using Xamarin.Forms;

namespace HackerNews
{
	public class NewsPage : BaseContentPage<NewsViewModel>
	{
		#region Constant Fields
		readonly ListView _storiesListView;
		#endregion

		public NewsPage() : base("Top Stories")
		{
			_storiesListView = new ListView(ListViewCachingStrategy.RecycleElement)
			{
				ItemTemplate = new DataTemplate(typeof(StoryTextCell)),
				IsPullToRefreshEnabled = true,
				BackgroundColor = Color.FromHex("F6F6EF"),
				SeparatorVisibility = SeparatorVisibility.None
			};
			_storiesListView.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.TopStoryList));
			_storiesListView.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsListRefreshing));
			_storiesListView.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.RefreshCommand));

			Content = _storiesListView;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			_storiesListView.BeginRefresh();
		}

		protected override void SubscribeEventHandlers()
		{
			_storiesListView.ItemTapped += HandleItemTapped;
			ViewModel.PullToRefreshFailed += HandlePullToRefreshFailed;
		}

		protected override void UnsubscribeEventHandlers()
		{
			_storiesListView.ItemTapped -= HandleItemTapped;
			ViewModel.PullToRefreshFailed -= HandlePullToRefreshFailed;
		}

		void HandleItemTapped(object sender, ItemTappedEventArgs e)
		{
			var listView = sender as ListView;
			var storyTapped = e.Item as StoryModel;

			Device.BeginInvokeOnMainThread(async () =>
			{
				listView.SelectedItem = null;
				await DependencyService.Get<IBrowserServices>()?.OpenBrowser(storyTapped?.Url);
			});
		}

		void HandlePullToRefreshFailed(object sender, string message) =>
			Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Update Failed", message, "OK"));
	}
}
