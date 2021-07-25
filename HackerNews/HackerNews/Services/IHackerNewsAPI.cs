using System.Collections.Generic;
using System.Threading.Tasks;

using Refit;

using HackerNews.Shared;

namespace HackerNews
{
    public interface IHackerNewsAPI
    {
        [Get("/topstories.json?print=pretty")]
        Task<IReadOnlyList<string>> GetTopStoryIDs();

        [Get("/item/{storyId}.json?print=pretty")]
        Task<StoryModel> GetStory(string storyId);
    }
}
