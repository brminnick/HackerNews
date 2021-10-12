using Foundation;
using Microsoft.Maui;

namespace HackerNews
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => Startup.Create();
    }
}