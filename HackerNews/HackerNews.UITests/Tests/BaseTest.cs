using NUnit.Framework;

using Xamarin.UITest;

namespace HackerNews.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        #region Constant Fields
        readonly Platform _platform;
        #endregion

        #region Constructors
        protected BaseTest(Platform platform) => _platform = platform;
        #endregion

        #region Properties
        protected IApp App { get; private set; }
        #endregion

        #region Methods
        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(_platform);

            App.Screenshot("App Launched");
        }
        #endregion
    }
}

