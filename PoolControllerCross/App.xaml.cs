using Autofac;
using eHub.Common.Api;
using eHub.Common.Services;
using PoolControllerCross.Services;
using PoolControllerCross.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PoolControllerCross
{
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            ConfigureContainer();

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

        void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var envInfo = new eHub.Common.Models.EnvironmentInfo[]
            {
                new eHub.Common.Models.EnvironmentInfo("staging", "http://192.168.0.17", 9000)
            };
            var config = new eHub.Common.Models.Configuration(envInfo, "staging");

            builder.RegisterInstance(config);
            builder.RegisterInstance(config.Environment);
            builder.RegisterModule(new PoolControllerModule());

            Container = builder.Build();
        }
    }
}
