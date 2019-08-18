using Xamarin.Forms;

namespace HackerNews
{
    public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel, new()
    {
        protected BaseContentPage(string pageTitle)
        {
            BindingContext = ViewModel;
            Title = pageTitle;
        }

        protected T ViewModel { get; } = new T();
    }
}
