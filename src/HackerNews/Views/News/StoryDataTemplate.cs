using CommunityToolkit.Maui.Markup;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace HackerNews;

public class StoryDataTemplate : DataTemplate
{
	public StoryDataTemplate() : base(CreateGrid)
	{

	}

	static Grid CreateGrid() => new()
	{
		RowSpacing = 1,

		RowDefinitions = Rows.Define(
			(Row.Title, 20),
			(Row.Description, 20),
			(Row.BottomPadding, 1)),

		Children =
		{
			new Label()
				.Row(Row.Title).Top()
				.Font(size: 16).TextColor(ColorConstants.TextCellTextColor)
				.Paddings(10, 0, 10, 0)
				.Bind(Label.TextProperty, nameof(StoryModel.Title)),

			new Label()
				.Row(Row.Description)
				.Font(size: 13).TextColor(ColorConstants.TextCellDetailColor)
				.Bind(Label.TextProperty, nameof(StoryModel.Description))
		}
	};

	enum Row { Title, Description, BottomPadding }
}