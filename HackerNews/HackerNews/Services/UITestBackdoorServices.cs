using Xamarin.Forms;

using Newtonsoft.Json;

namespace HackerNews
{
    public static class UITestBackdoorServices
    {
        public static string GetSerializedStoryList()
        {
            var storyList = GetViewModel().TopStoryList;
            return JsonConvert.SerializeObject(storyList);
        }

        static NewsPage GetNewsPage()
        {
            var mainPage = Application.Current.MainPage as NavigationPage;
            return mainPage.RootPage as NewsPage;
        }

        static NewsViewModel GetViewModel() => GetNewsPage().BindingContext as NewsViewModel;

    }
}