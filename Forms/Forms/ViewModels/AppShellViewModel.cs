using Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Forms.ViewModels
{
    class AppShellViewModel
    {
        readonly INavigation navigation;
        Random rand = new Random();
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }

        public ICommand HelpCommand => new Command<string>((url) => Device.OpenUri(new Uri(url)));
        public ICommand RandomPageCommand => new Command(async () => await NavigateToRandomPageAsync().ConfigureAwait(true));

        public AppShellViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }

        void RegisterRoutes()
        {
            routes.Add("monkeydetails", typeof(CrossWordPage));
            routes.Add("beardetails", typeof(FingerPaintingPage));
            routes.Add("catdetails", typeof(WatermarkPage));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

        async Task NavigateToRandomPageAsync()
        {
            string destinationRoute = routes.ElementAt(rand.Next(0, routes.Count)).Key;
            string animalName = null;

            switch (destinationRoute)
            {
                //case "monkeydetails":
                //    animalName = MonkeyData.Monkeys.ElementAt(rand.Next(0, MonkeyData.Monkeys.Count)).Name;
                //    break;
                //case "beardetails":
                //    animalName = BearData.Bears.ElementAt(rand.Next(0, BearData.Bears.Count)).Name;
                //    break;
                //case "catdetails":
                //    animalName = CatData.Cats.ElementAt(rand.Next(0, CatData.Cats.Count)).Name;
                //    break;
                //case "dogdetails":
                //    animalName = DogData.Dogs.ElementAt(rand.Next(0, DogData.Dogs.Count)).Name;
                //    break;
                //case "elephantdetails":
                //    animalName = ElephantData.Elephants.ElementAt(rand.Next(0, ElephantData.Elephants.Count)).Name;
                //    break;
            }

            ShellNavigationState state = Shell.Current.CurrentState;
            await Shell.Current.GoToAsync($"{state.Location}/{destinationRoute}?name={animalName}").ConfigureAwait(false);
            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
