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

            var ctx = new PoolAndSpaViewModel();
            BindingContext = new PoolAndSpaViewModel()
            {
                IsBusy = true,
            };

            ToolbarItems.Add(new ToolbarItem("Refresh", "ic_refresh_white_24dp.png", async () =>
            {
                await Refresh();
            }));

            InitAsync();
        }

        async void InitAsync()
        {
            await Refresh();
        }

        async Task Refresh()
        {
            BindingContext = new PoolAndSpaViewModel()
            {
                IsBusy = true,
            };

            var (statuses, poolSched, glSched, plSched) = await Task.Run(async () =>
            {
                var status = (await _poolService.GetAllStatuses()).ToList();
                var sched = await _poolService.GetSchedule();
                var groundLightSched = await _poolService.GetGroundLightSchedule();
                var poolLightSched = await _poolService.GetPoolLightSchedule();

                return (status, sched, groundLightSched, poolLightSched);
            });

            var pool = statuses.FirstOrDefault(_ => _.PinNumber == Pin.PoolPump);
            var booster = statuses.FirstOrDefault(_ => _.PinNumber == Pin.BoosterPump);
            var spaPump = statuses.FirstOrDefault(_ => _.PinNumber == Pin.SpaPump);
            var heater = statuses.FirstOrDefault(_ => _.PinNumber == Pin.Heater);

            var poolModel = new PoolPumpViewModel(poolSched, pool);
            var boosterPumpModel = new EquipmentViewModel(booster);
            var heaterModel = new EquipmentViewModel(heater);

            BindingContext = new PoolAndSpaViewModel(poolModel, boosterPumpModel, heaterModel)
            {
                IsBusy = false,
            };
        }
    }
}