using Forms.Utility;
using Forms.ViewModels;
using System;

using Xamarin.Forms;

namespace Forms.Views
{
    public partial class AppShell : Shell
    {
        long lastPress;
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel(Navigation);
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