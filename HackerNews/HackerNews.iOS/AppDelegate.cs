using UIKit;
using Foundation;
using System.Diagnostics;

namespace HackerNews.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
			ExposeAutomationAPIs();

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        [Conditional("DEBUG")]
        void ExposeAutomationAPIs() => Xamarin.Calabash.Start();
    }
}
