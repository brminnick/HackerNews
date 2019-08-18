using NUnit.Framework;

using Xamarin.UITest;

namespace HackerNews.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    abstract class BaseTest
    {
        readonly Platform _platform;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp App { get; private set; }
        protected NewsPage NewsPage { get; private set; }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            App = AppInitializer.StartApp(_platform);
            App.Screenshot("App Initialized");

            NewsPage = new NewsPage(App);

            NewsPage.WaitForPageToLoad();
        }
    }
}

