using Forms.Models;
using Forms.Views;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Forms
{
    public partial class App : Application
    {
        public static double ScreenWidth;
        public static double ScreenHeight;
        private static SQLiteConnection dbConnection;

        public static SQLiteConnection GetDbConnection
        {
            get
            {
                if (dbConnection == null)
                {
                    string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "forms_db.db3");
                    dbConnection = new SQLiteConnection(dbPath);
                    dbConnection.CreateTable<ItemsDB>();
                    dbConnection.CreateTable<WatermarkDB>();
                    dbConnection.CreateTable<User>();
                }
                return dbConnection;
            }
        }

        public App()
        {
            InitializeComponent();
            //MainPage = new AppShell();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
