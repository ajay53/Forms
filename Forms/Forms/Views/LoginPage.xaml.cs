using Forms.Utility;
using Forms.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        long lastPress;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
        }

        protected override bool OnBackButtonPressed()
        {
            long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            if (currentTime - lastPress > 3000)
            {
                UtilityFunction.ToastMessage("Press back again to exit");
                lastPress = currentTime;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}