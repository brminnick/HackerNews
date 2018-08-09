using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HackerNews
{
	public class NewsViewModel : BaseViewModel
	{
		#region Fields
		bool _isListRefreshing;
		ICommand _refreshCommand;
		List<StoryModel> _topStoryList;
		#endregion

		#region Events
		public event EventHandler<string> PullToRefreshFailed;
		#endregion

		#region Properties
		public ICommand RefreshCommand => _refreshCommand ??
			(_refreshCommand = new Command(async () => await ExecuteRefreshCommand().ConfigureAwait(false)));

		public List<StoryModel> TopStoryList
		{
			get => _topStoryList;
			set => SetProperty(ref _topStoryList, value);
		}

		public bool IsListRefreshing
		{
			get => _isListRefreshing;
			set => SetProperty(ref _isListRefreshing, value);
		}
		#endregion

		#region Methods
		async Task ExecuteRefreshCommand()
		{
			IsListRefreshing = true;

			try
			{
				var topStoryList = await GetTopStories(20).ConfigureAwait(false);

				var topStoryTitleList = topStoryList.Select(x => x.Title).ToList();
				var sentimentResults = await TextAnalysisService.GetSentiment(topStoryTitleList).ConfigureAwait(false);

				foreach (var sentimentResult in sentimentResults)
				{
					var story = topStoryList.Where(x => x.Title.Equals(sentimentResult.Key))?.FirstOrDefault();
					story.TitleSentimentScore = sentimentResult.Value;
				}

				TopStoryList = topStoryList;
			}
			catch (TaskCanceledException)
			{
				OnPullToRefreshFailed("Http Timeout");
			}
			finally
			{
				IsListRefreshing = false;
			}
		}

		async Task<List<StoryModel>> GetTopStories(int numberOfStories)
		{
			var topStoryIds = await HackerNewsAPIService.GetTopStoryIDs().ConfigureAwait(false);

			var getTop20StoriesTaskList = new List<Task<StoryModel>>();
			for (int i = 0; i < numberOfStories; i++)
			{
				getTop20StoriesTaskList.Add(HackerNewsAPIService.GetStory(topStoryIds[i]));
			}

			var topStoriesArray = await Task.WhenAll(getTop20StoriesTaskList).ConfigureAwait(false);


            return topStoriesArray.OrderByDescending(x => x.Score).ToList();
		}

		void OnPullToRefreshFailed(string message) => PullToRefreshFailed?.Invoke(this, message);
		#endregion
	}
}
