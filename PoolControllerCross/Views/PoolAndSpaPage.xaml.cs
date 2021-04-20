using Autofac;
using eHub.Common.Models;
using eHub.Common.Services;
using PoolControllerCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolControllerCross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PoolAndSpaPage : TabbedPage
    {
        readonly IPoolService _poolService;

        public PoolAndSpaPage()
        {
            InitializeComponent();

            _poolService = App.Container.Resolve<IPoolService>();

            GetSchedule();
        }

        async void GetSchedule()
        {
            await GetScheduleAndSetContext();
        }

        async Task GetScheduleAndSetContext()
        {
            BindingContext = new PoolAndSpaViewModel()
            {
                IsBusy = true,
            };

            var statuses = await _poolService.GetAllStatuses();
            var poolSched = await _poolService.GetSchedule();
            var pool = statuses.FirstOrDefault(_ => _.PinNumber == Pin.PoolPump);
            var booster = statuses.FirstOrDefault(_ => _.PinNumber == Pin.BoosterPump);
            var spaPump = statuses.FirstOrDefault(_ => _.PinNumber == Pin.SpaPump);
            var heater = statuses.FirstOrDefault(_ => _.PinNumber == Pin.Heater);

            if (poolSched == null || statuses == null)
                return;

            var poolModel = new PoolPumpViewModel(poolSched, pool);
            var boosterPumpModel = new BoosterPumpViewModel(booster);
            var heaterModel = new HeaterViewModel(heater);

            BindingContext = new PoolAndSpaViewModel(poolModel, boosterPumpModel, heaterModel)
            {
                IsBusy = false,
            };
        }
    }
}