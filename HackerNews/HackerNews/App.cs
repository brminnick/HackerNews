using Microsoft.Maui.Controls;

namespace HackerNews
{
    public class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            var navigationPage = new NavigationPage(new NewsPage())
            {
                BarBackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                BarTextColor = ColorConstants.NavigationBarTextColor
            };

            MainPage = navigationPage;
        }
    }
}
