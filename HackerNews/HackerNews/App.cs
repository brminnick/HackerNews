using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace HackerNews
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            Xamarin.Forms.Device.SetFlags(new[] { "Markup_Experimental" });

            var navigationPage = new Xamarin.Forms.NavigationPage(new NewsPage())
            {
                BarBackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                BarTextColor = ColorConstants.NavigationBarTextColor
            };

            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }
    }
}
