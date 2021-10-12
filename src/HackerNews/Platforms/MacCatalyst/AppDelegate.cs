using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace HackerNews;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.Create();
}