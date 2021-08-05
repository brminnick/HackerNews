using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace HackerNews
{
    public class App : Application
    {
        protected override Window CreateWindow(IActivationState activationState)
        {
            var newsPage = ServiceProvider.GetService<NewsPage>();

            var navigationPage = new NavigationPage(newsPage)
            {
                BarBackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                BarTextColor = ColorConstants.NavigationBarTextColor
            };

            return new Window(navigationPage);
        }
    }
}
