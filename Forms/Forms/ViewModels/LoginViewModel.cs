using Forms.Models;
using Forms.Utility;
using Forms.Views;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Forms.ViewModels
{
    class LoginViewModel
    {
        readonly INavigation navigation;
        public User User { get; set; }
        public LoginViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            User = new User("","");
        }

        public Command LoginTappedCommand => new Command(() =>
        {
            if (!AreFieldsEmpty())
            {
                User user = FetchUser(User.Username);
                if (user != null)
                {
                    //check password
                    if(User.Password == user.Password)
                    {
                        Application.Current.MainPage = new AppShell();
                        Preferences.Set(Constant.IS_LOGGED_IN, Constant.TRUE);
                        Preferences.Set(Constant.USERNAME, user.Username);
                        Preferences.Set(Constant.PASSWORD, user.Password);
                    }
                    else
                    {
                        UtilityFunction.ToastMessage("Incorrect Password");
                    }
                }
                else
                {
                    UtilityFunction.ToastMessage("User does not exist, Please register");
                }
            }
            else
            {
                UtilityFunction.ToastMessage("Please fill all fields");
            }
        });

        public Command RegisterTappedCommand => new Command(() =>
        {
            navigation.PushAsync(new RegistrationPage());
        });

        private bool AreFieldsEmpty()
        {
            Console.WriteLine(User.ToString());
            if (User.Username.Trim().Length == 0 || User.Password.Trim().Length == 0)
            {
                return true;
            }
            return false;
        }

        private User FetchUser(string username)
        {
            User user = null;
            try
            {
                user = App.GetDbConnection.Get<User>(username);
                Console.WriteLine($"data: {user}");
            }
            catch (Exception)
            {
                Console.WriteLine($"data: {user}");
                return user;
            }
            return user;
        }
    }
}
