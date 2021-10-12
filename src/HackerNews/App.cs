using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace HackerNews
{
    class App : Application
    {
        public App(NewsPage newsPage) => MainPage = new NavigationPage(newsPage)
        {
            BarBackgroundColor = ColorConstants.NavigationBarBackgroundColor,
            BarTextColor = ColorConstants.NavigationBarTextColor
        };
    }
}
