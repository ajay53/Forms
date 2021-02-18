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

        public AppShellViewModel()
        {
            RegisterRoutes();
        }

        void RegisterRoutes()
        {
            routes.Add("ClickPhoto", typeof(CLickPhotoPage));
            routes.Add("Watermark", typeof(WatermarkPage));
            routes.Add("Home", typeof(HomePage));
            routes.Add("CrossWord", typeof(CrossWordPage));
            routes.Add("FingerPainting", typeof(FingerPaintingPage));
            routes.Add("ReadJson", typeof(ReadJsonPage));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }

            //Task.Run(async () => 
            //{
            //    await Shell.Current.GoToAsync("Home");
            //});
        }
        
    }
}
