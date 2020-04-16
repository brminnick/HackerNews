using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using HackerNews.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HackerNews
{
    public class NewsViewModel : BaseViewModel
    {
        readonly WeakEventManager<string> _pullToRefreshEventManager = new WeakEventManager<string>();

        bool _isListRefreshing;
        ICommand? _refreshCommand;

        public NewsViewModel()
        {
            //Ensure Observable Collection is thread-safe https://codetraveler.io/2019/09/11/using-observablecollection-in-a-multi-threaded-xamarin-forms-application/
            BindingBase.EnableCollectionSynchronization(TopStoryCollection, null, ObservableCollectionCallback);
        }

        public event EventHandler<string> PullToRefreshFailed
        {
            add => _pullToRefreshEventManager.AddEventHandler(value);
            remove => _pullToRefreshEventManager.RemoveEventHandler(value);
        }

        public ObservableCollection<StoryModel> TopStoryCollection { get; } = new ObservableCollection<StoryModel>();

        public ICommand RefreshCommand => _refreshCommand ??= new AsyncCommand(ExecuteRefreshCommand);

        public bool IsListRefreshing
        {
            get => _isListRefreshing;
            set => SetProperty(ref _isListRefreshing, value);
        }

        async Task ExecuteRefreshCommand()
        {
            TopStoryCollection.Clear();

            try
            {
                await foreach (var story in GetTopStories(StoriesConstants.NumberOfStories).ConfigureAwait(false))
                {
                    try
                    {
                        story.TitleSentiment = await TextAnalysisService.GetSentiment(story.Title).ConfigureAwait(false);
                    }
                    catch
                    {
                        //Todo Add TextAnalysis API Key in TextAnalysisConstants.cs
                    }
                    finally
                    {
                        if (!TopStoryCollection.Any(x => x.Title.Equals(story.Title)))
                            InsertIntoSortedCollection(TopStoryCollection, (a, b) => b.Score.CompareTo(a.Score), story);
                    }
                }
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

        async IAsyncEnumerable<StoryModel> GetTopStories(int? storyCount = int.MaxValue)
        {
            var topStoryIds = await HackerNewsAPIService.GetTopStoryIDs().ConfigureAwait(false);
            var getTopStoryTaskList = topStoryIds.Select(HackerNewsAPIService.GetStory).ToList();

            while (getTopStoryTaskList.Any() && storyCount-- > 0)
            {
                var completedGetStoryTask = await Task.WhenAny(getTopStoryTaskList).ConfigureAwait(false);
                getTopStoryTaskList.Remove(completedGetStoryTask);

                yield return await completedGetStoryTask.ConfigureAwait(false);
            }
        }

        void InsertIntoSortedCollection<T>(in ObservableCollection<T> collection, in Comparison<T> comparison, in T modelToInsert)
        {
            if (collection.Count is 0)
            {
                collection.Add(modelToInsert);
            }
            else
            {
                int index = 0;
                foreach (var model in collection)
                {
                    if (comparison(model, modelToInsert) >= 0)
                    {
                        collection.Insert(index, modelToInsert);
                        return;
                    }

                    index++;
                }
            }
        }

        //Ensure Observable Collection is thread-safe https://codetraveler.io/2019/09/11/using-observablecollection-in-a-multi-threaded-xamarin-forms-application/
        void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            MainThread.BeginInvokeOnMainThread(accessMethod);
        }

        void OnPullToRefreshFailed(string message) => _pullToRefreshEventManager.HandleEvent(this, message, nameof(PullToRefreshFailed));
    }
}
