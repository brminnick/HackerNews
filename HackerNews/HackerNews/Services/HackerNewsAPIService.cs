using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Polly;
using Refit;

using HackerNews.Shared;

namespace HackerNews
{
    public static class HackerNewsAPIService
    {
        readonly static Lazy<IHackerNewsAPI> _hackerNewsApiClientHolder = new Lazy<IHackerNewsAPI>(RestService.For<IHackerNewsAPI>("https://hacker-news.firebaseio.com/v0"));

        static IHackerNewsAPI HackerNewsApiClient => _hackerNewsApiClientHolder.Value;

        public static Task<StoryModel> GetStory(string storyId) => AttemptAndRetry(() => HackerNewsApiClient.GetStory(storyId));
        public static Task<IReadOnlyList<string>> GetTopStoryIDs() => AttemptAndRetry(() => HackerNewsApiClient.GetTopStoryIDs());

        static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);

            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
