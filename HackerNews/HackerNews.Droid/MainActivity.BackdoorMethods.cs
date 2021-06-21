#if DEBUG
using Android.Runtime;
using HackerNews.Shared;
using Newtonsoft.Json;

namespace HackerNews.Droid
{
    public partial class MainActivity
    {
        [Preserve, Java.Interop.Export(BackdoorConstants.GetSerializedStoryList)]
        public string GetSerializedStoryList()
        {
            var storyList = UITestBackdoorServices.GetStoryList();
            return JsonConvert.SerializeObject(storyList);
        }
    }
}
#endif
