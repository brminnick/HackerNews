#if DEBUG
using Foundation;
using HackerNews.Shared;
using Newtonsoft.Json;

namespace HackerNews.iOS
{
    public partial class AppDelegate
    {
        public AppDelegate()
        {
            Xamarin.Calabash.Start();
        }

        [Preserve, Export(BackdoorConstants.GetSerializedStoryList + ":")]
        public NSString GetSerializedStoryList(NSString noValue)
        {
            var storyList = UITestBackdoorServices.GetStoryList();
            return new NSString(JsonConvert.SerializeObject(storyList));
        }
    }
}
#endif
