using Forms.Utility;
using Forms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Forms.ViewModels
{
    class LoginViewModel
    {
        readonly INavigation navigation;
        public LoginViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }

        public Command LoginTappedCommand => new Command(() =>
        {
            //UtilityFunction.ToastMessage("Login Tapped");
            Application.Current.MainPage = new AppShell();
        });

        public Command RegisterTappedCommand => new Command(() =>
        {
            UtilityFunction.ToastMessage("Register Tapped");
        });
    }
}
