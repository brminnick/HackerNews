using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

using HackerNews.Shared;

namespace HackerNews.UITests
{
    public class NewsPage : BasePage
    {
        public NewsPage(IApp app) : base(app, PageTitleConstants.NewsPageTitle)
        {
        }

        public bool IsRefreshViewRefreshIndicatorDisplayed => App switch
        {
            AndroidApp androidApp => (bool)androidApp.Query(x => x.Class("RefreshViewRenderer").Invoke("isRefreshing")).First(),
            IApp iOSApp => iOSApp.Query(x => x.Class("UIRefreshControl")).Any(),
            _ => throw new NotSupportedException("Xamarin.UITest only supports Android and iOS"),
        };

        public bool IsBrowserOpen => App switch
        {
            iOSApp iOSApp => iOSApp.Query(x => x.Class("SFSafariView")).Any(),
            _ => throw new NotSupportedException("Browser Can Only Be Verified on iOS")
        };

        public void WaitForNoActivityIndicator(int timeoutInSeconds = 60)
        {
            int counter = 0;
            while (IsRefreshViewRefreshIndicatorDisplayed && counter < timeoutInSeconds)
            {
                Thread.Sleep(1000);
                counter++;

                if (counter >= timeoutInSeconds)
                    throw new Exception($"Loading the list took longer than {timeoutInSeconds}s");
            }
        }

        public override void WaitForPageToLoad()
        {
            base.WaitForPageToLoad();

            WaitForNoActivityIndicator();
        }

        public IReadOnlyList<StoryModel> GetStoryList()
        {
            var serializedStoryList = App switch
            {
                iOSApp iosApp => iosApp.Invoke("getSerializedStoryList:", "").ToString(),
                AndroidApp androidApp => androidApp.Invoke("GetSerializedStoryList").ToString(),
                _ => throw new NotSupportedException("Platform Not Supported"),
            };

            return Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<StoryModel>>(serializedStoryList);
        }
    }
}
