using eHub.Common.Api;
using eHub.Common.Services;
using PoolControllerCross.Services;
using PoolControllerCross.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolControllerCross
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();


            DependencyService.Register<WebInterface>();
            DependencyService.Register<PoolApi>();
            DependencyService.Register<PoolService>();

            MainPage = new AppShell();
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
