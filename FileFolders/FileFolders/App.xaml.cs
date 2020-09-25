using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileFolders
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new View.Login_Screen());
            bool isLoggedIn = Current.Properties.ContainsKey("IsLoggedIn") ? Convert.ToBoolean(Current.Properties["IsLoggedIn"]) : false;
            if (!isLoggedIn)
            {
                //Load if Not Logged In
                MainPage = new NavigationPage(new View.Login_Screen());
            }
            else
            {
                MainPage = new MainPage();
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
