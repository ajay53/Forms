using Forms.Utility;
using System.Windows.Input;
using Xamarin.Forms;

namespace Forms.ViewModels
{

    class HomePageViewModel
    {
        public Command TitleTappedCommand{ get; set; }
        StackLayout parentStack;

        public HomePageViewModel(View parentView)
        {
            TitleTappedCommand = new Command(OnTitleTapped);
            parentStack = parentView as StackLayout;

            Initialize();
        }

        private void Initialize()
        {
            //parentStack.Children.Add(new Label { Text = "asdfgh" });
        }

        public string LabelText { get; set; }

        private void OnTitleTapped()
        {
            UtilityFunction.ToastMessage("asdfg");
        }

        public Command _TitleTappedCommand => new Command(() =>
        {
            Shell.Current.GoToAsync("CrossWord");
        });
    }
}
