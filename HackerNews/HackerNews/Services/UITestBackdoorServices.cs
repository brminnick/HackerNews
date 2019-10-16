using Xamarin.Forms;

using Newtonsoft.Json;

namespace HackerNews
{
    public static class UITestBackdoorServices
    {
        public static string GetSerializedStoryList()
        {
            var storyList = GetViewModel().TopStoryCollection;
            return JsonConvert.SerializeObject(storyList);
        }

        static NewsPage GetNewsPage()
        {
            var mainPage = (NavigationPage)Application.Current.MainPage;
            return (NewsPage)mainPage.RootPage;
        }

        static NewsViewModel GetViewModel() => (NewsViewModel)GetNewsPage().BindingContext;
    }
}