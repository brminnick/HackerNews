using System;
using NUnit.Framework;

using Xamarin.UITest;

namespace HackerNews.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    abstract class BaseTest
    {
        readonly Platform _platform;

        IApp? _app;
        NewsPage? _newsPage;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp App => _app ?? throw new NullReferenceException();
        protected NewsPage NewsPage => _newsPage ?? throw new NullReferenceException();

        [SetUp]
        public virtual void BeforeEachTest()
        {
            _app = AppInitializer.StartApp(_platform);
            _app.Screenshot("App Initialized");

            _newsPage = new NewsPage(App);
            _newsPage.WaitForPageToLoad();
        }
    }
}

