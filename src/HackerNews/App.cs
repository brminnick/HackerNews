using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace HackerNews;

class App : Microsoft.Maui.Controls.Application
{
	public App(NewsPage newsPage)
	{
		var navigationPage = new Microsoft.Maui.Controls.NavigationPage(newsPage)
		{
			BarBackgroundColor = ColorConstants.NavigationBarBackgroundColor,
			BarTextColor = ColorConstants.NavigationBarTextColor
		};

		navigationPage.On<iOS>().SetPrefersLargeTitles(true);

		MainPage = navigationPage;
	}
}