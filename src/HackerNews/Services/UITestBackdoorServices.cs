#if DEBUG
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace HackerNews
{
    public static class UITestBackdoorServices
    {
        public static IReadOnlyList<StoryModel> GetStoryList() => GetViewModel().TopStoryCollection;

        static NewsPage GetNewsPage()
        {
            var mainPage = (NavigationPage)Application.Current.MainPage;
            return (NewsPage)mainPage.RootPage;
        }

        static NewsViewModel GetViewModel() => (NewsViewModel)GetNewsPage().BindingContext;
    }
}
#endif