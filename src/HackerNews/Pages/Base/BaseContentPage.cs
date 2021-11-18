using Microsoft.Maui.Controls;

namespace HackerNews;

public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel
{
	protected BaseContentPage(T viewModel, string pageTitle)
	{
		BindingContext = ViewModel = viewModel;
		Title = pageTitle;
	}

	protected T ViewModel { get; }
}