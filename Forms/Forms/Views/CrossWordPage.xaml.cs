using CrossWordHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrossWordPage : ContentPage
    {
        readonly List<string> wordList = new List<string>();
        GridButton gridButton;
        readonly int gridMatrixSize = 4;
        readonly double gridButtonSize;

        public CrossWordPage()
        {
            InitializeComponent();
            BindingContext = new CrossWordDrawLineViewModel(canvas);
            gridButtonSize = App.ScreenWidth / gridMatrixSize;

            wordList.Add("ABC");
            wordList.Add("DE");
            wordList.Add("AEI");

            CreateGrid();
            CreateCanvas();
            AddButtons();
            clearCanvasButton.Clicked += ClearCanvasButton_Clicked;
        }
        
        private void ClearCanvasButton_Clicked(object sender, EventArgs e)
        {
            CrossWordDrawLineViewModel.clearCanvas = true;
            new MessageCenterHelper().SendMessageMenu(null);
        }

        void CreateGrid()
        {

            grid.Opacity = 0.5;

            for (int i = 0; i < gridMatrixSize; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(gridButtonSize) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(gridButtonSize) });
            }

            AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(grid);
        }

        void CreateCanvas()
        {
            AbsoluteLayout.SetLayoutBounds(canvas, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(canvas, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(canvas);
        }

        void AddButtons()
        {
            char buttonText = 'A';
            int gridElementPosition = 1;

            for (int row = 0; row < gridMatrixSize; row++)
            {
                for (int column = 0; column < gridMatrixSize; column++)
                {
                    gridButton = new GridButton
                    {
                        BackgroundColor = Color.LightBlue,
                        Text = buttonText.ToString(),
                        Padding = 0,
                        CornerRadius = 0
                    };

                    gridButton.row = row;
                    gridButton.column = column;
                    gridButton.Effects.Add(Effect.Resolve("XamarinDocs.FocusEffect"));
                    grid.Children.Add(gridButton, column, row);

                    buttonText++;
                    gridElementPosition++;

                }
            }
        }
    }
}