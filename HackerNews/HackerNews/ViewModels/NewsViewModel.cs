using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;

using HackerNews.Shared;

namespace HackerNews
{
    public class NewsViewModel : BaseViewModel
    {
        readonly WeakEventManager<string> _pullToRefreshEventManager = new WeakEventManager<string>();

        bool _isListRefreshing;
        ICommand _refreshCommand;
        List<StoryModel> _topStoryList;

        public event EventHandler<string> PullToRefreshFailed
        {
            add => _pullToRefreshEventManager.AddEventHandler(value);
            remove => _pullToRefreshEventManager.RemoveEventHandler(value);
        }

        public ICommand RefreshCommand => _refreshCommand ??
            (_refreshCommand = new AsyncCommand(ExecuteRefreshCommand));

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

        async Task ExecuteRefreshCommand()
        {
            try
            {
                var topStoryList = await GetTopStories(StoriesConstants.NumberOfStories).ConfigureAwait(false);

                var topStoryTitleList = topStoryList.Select(x => x.Title).ToList();
                var sentimentResults = await TextAnalysisService.GetSentiment(topStoryTitleList).ConfigureAwait(false);

                foreach (var sentimentResult in sentimentResults)
                {
                    var story = topStoryList.First(x => x.Title.Equals(sentimentResult.Key));
                    story.TitleSentimentScore = sentimentResult.Value;
                }

                TopStoryList = topStoryList;
            }
            catch (Exception e)
            {
                OnPullToRefreshFailed(e.Message);
            }
            finally
            {
                IsListRefreshing = false;
            }
        }

        async Task<List<StoryModel>> GetTopStories(int? maximumNumberOfStories = null)
        {
            var topStoryIds = await HackerNewsAPIService.GetTopStoryIDs().ConfigureAwait(false);

            var getTopStoriesTaskList = new List<Task<StoryModel>>(topStoryIds.Select(HackerNewsAPIService.GetStory));

            var topStoriesArray = await Task.WhenAll(getTopStoriesTaskList).ConfigureAwait(false);

            return topStoriesArray.OrderByDescending(x => x.Score).Take(maximumNumberOfStories ?? int.MaxValue).ToList();
        }

        void OnPullToRefreshFailed(string message) => _pullToRefreshEventManager.HandleEvent(this, message, nameof(PullToRefreshFailed));
    }
}
