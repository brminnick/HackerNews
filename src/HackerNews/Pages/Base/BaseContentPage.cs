using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;

namespace HackerNews;

public abstract class BaseContentPage<T> : ContentPage where T : ObservableObject
{
	protected BaseContentPage(T viewModel, string pageTitle)
	{
		Title = pageTitle;
		base.BindingContext = viewModel;
	}

	protected new T BindingContext => (T)base.BindingContext;
}