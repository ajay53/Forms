using Forms.Models;
using Forms.Utility;
using System;
using Xamarin.Forms;

namespace Forms.ViewModels
{
    class RegistrationViewModel : BaseViewModel
    {
        INavigation navigation;
        public RegistrationViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            User = new User();
        }

        public User User{ get; set; }

        public Command RegisterTappedCommand => new Command(() =>
        {
            if (!AreFieldsEmpty())
            {
                if (!IsUserPresent(User.Username))
                {
                    //insert into db
                    App.GetDbConnection.Insert(User);
                    navigation.PopAsync();
                }
                else
                {
                    UtilityFunction.ToastMessage("User Already Registered");
                }
            }
        });

        private bool AreFieldsEmpty()
        {
            Console.WriteLine(User.ToString());
            if(User.Username.Trim().Length == 0 || User.EmailId.Trim().Length == 0 || User.Password.Trim().Length == 0)
            {
                return true;
            }
            return false;
        }
        
        private bool IsUserPresent(string username)
        {
            User data = null;
            try
            {
                data = App.GetDbConnection.Get<User>(username);
                Console.WriteLine($"data: {data}");
            }
            catch (Exception)
            {
                Console.WriteLine($"data: {data}");
                return false;
            }
            return true;
        }
    }
}
