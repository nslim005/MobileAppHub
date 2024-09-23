using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics
{
    public partial class App : Application
    {

        public static string DatabaseLocation = string.Empty;

        public App ()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }


       
        public App(string dataBaseLocation)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            DatabaseLocation = dataBaseLocation;
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

