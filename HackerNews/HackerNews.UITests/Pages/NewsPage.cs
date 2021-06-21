using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HackerNews.Shared;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace HackerNews.UITests
{
    public class NewsPage : BasePage
    {
        public NewsPage(IApp app) : base(app, PageTitleConstants.NewsPageTitle)
        {
        }

        public IReadOnlyList<StoryModel> StoryList => App.InvokeBackdoorMethod<IReadOnlyList<StoryModel>>(BackdoorConstants.GetSerializedStoryList);

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

        public override void WaitForPageToLoad()
        {
            base.WaitForPageToLoad();

            WaitForNoActivityIndicator();
        }

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
    }
}
