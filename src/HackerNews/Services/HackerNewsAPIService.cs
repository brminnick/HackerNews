using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polly;
using Refit;

namespace HackerNews;

class HackerNewsAPIService
{
	readonly IHackerNewsAPI _hackerNewsClient;

	public HackerNewsAPIService(IHackerNewsAPI hackerNewslient) => _hackerNewsClient = hackerNewslient;

	public Task<StoryModel> GetStory(long storyId) => AttemptAndRetry(() => _hackerNewsClient.GetStory(storyId));
	public Task<IReadOnlyList<long>> GetTopStoryIDs() => AttemptAndRetry(() => _hackerNewsClient.GetTopStoryIDs());

	static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3)
	{
		return Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);

		static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
	}
}
