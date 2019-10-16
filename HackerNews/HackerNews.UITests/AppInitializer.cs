using System;

using Xamarin.UITest;

namespace HackerNews.UITests
{
    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform) => platform switch
        {
            Platform.Android => ConfigureApp.Android.StartApp(),
            Platform.iOS => ConfigureApp.iOS.StartApp(),
            _ => throw new NotSupportedException(),
        };
    }
}
