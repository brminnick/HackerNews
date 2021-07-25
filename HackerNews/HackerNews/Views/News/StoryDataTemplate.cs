using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace HackerNews
{
    public class StoryDataTemplate : DataTemplate
    {
        public StoryDataTemplate() : base(CreateGrid)
        {

        }

        static Grid CreateGrid() => new Grid
        {
            RowSpacing = 1,

            RowDefinitions = Rows.Define(
                (Row.Title, 20),
                (Row.Description, 20),
                (Row.BottomPadding, 1)),

            Children =
            {
                new TitleLabel().Row(Row.Title)
                    .Bind(Label.TextProperty, nameof(StoryModel.Title)),
                new DescriptionLabel().Row(Row.Description)
                    .Bind(Label.TextProperty, nameof(StoryModel.Description))
            }
        };

        enum Row { Title, Description, BottomPadding }

        class TitleLabel : Label
        {
            public TitleLabel()
            {
                FontSize = 16;
                TextColor = ColorConstants.TextCellTextColor;

                VerticalTextAlignment = TextAlignment.Start;

                Padding = new Thickness(10, 0);
            }
        }

        class DescriptionLabel : Label
        {
            public DescriptionLabel()
            {
                FontSize = 13;
                TextColor = ColorConstants.TextCellDetailColor;

                Padding = new Thickness(10, 0, 10, 5);
            }
        }
    }
}