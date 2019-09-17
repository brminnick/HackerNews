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

        public bool IsRefreshActivityIndicatorDisplayed
        {
            get
            {
                switch (App)
                {
                    case AndroidApp androidApp:
                        return (bool)App.Query(x => x.Class("ListViewRenderer_SwipeRefreshLayoutWithFixedNestedScrolling").Invoke("isRefreshing")).FirstOrDefault();

                    case iOSApp iosApp:
                        return App.Query(x => x.Class("UIRefreshControl")).Any();

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public void WaitForNoActivityIndicator(int timeoutInSeconds = 60)
        {
            int counter = 0;
            while (IsRefreshActivityIndicatorDisplayed && counter < timeoutInSeconds)
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

        public List<StoryModel> GetStoryList()
        {
            string serializedStoryList;

            switch (App)
            {
                case iOSApp iosApp:
                    serializedStoryList = iosApp.Invoke("getSerializedStoryList:", "").ToString();
                    break;
                case AndroidApp androidApp:
                    serializedStoryList = androidApp.Invoke("GetSerializedStoryList").ToString();
                    break;
                default:
                    throw new NotSupportedException("Platform Not Supported");
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<StoryModel>>(serializedStoryList);
        }
    }
}
