using UIKit;
using Foundation;

namespace HackerNews.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Xamarin.Calabash.Start();

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        [Preserve, Export("getSerializedStoryList:")]
        public NSString GetSerializedStoryList(NSString noValue) =>
            new NSString(UITestBackdoorServices.GetSerializedStoryList());
    }
}
