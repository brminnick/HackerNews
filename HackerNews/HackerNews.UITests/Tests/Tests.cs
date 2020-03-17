using System.Linq;
using HackerNews.Shared;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace HackerNews.UITests
{
    class Tests : BaseTest
    {
        public Tests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void AppLaunches()
        {

        }

        [Test]
        public void DownloadTopStories()
        {
            //Arrange
            int actualNumberOfStories;
            const int requestedNumberOfStories = StoriesConstants.NumberOfStories;

            //Act
            actualNumberOfStories = NewsPage.GetStoryList().Count;

            //Assert
            Assert.GreaterOrEqual(requestedNumberOfStories, actualNumberOfStories);
            Assert.Greater(actualNumberOfStories, 0);
        }

        [Test]
        public void ReadStory()
        {
            //Arrange
            var topStory = NewsPage.GetStoryList().First();

            //Act
            App.Tap(topStory.Title);

            //Assert
            if (App is iOSApp)
            {
                Assert.IsTrue(NewsPage.IsBrowserOpen);
            }
        }
    }
}
