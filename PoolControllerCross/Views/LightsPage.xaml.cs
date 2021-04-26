using Autofac;
using eHub.Common.Models;
using eHub.Common.Services;
using PoolControllerCross.Helpers;
using PoolControllerCross.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoolControllerCross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LightsPage: TabbedPage
    {
        readonly IPoolService _poolService;
        readonly IDialogService _dialogService;

        public LightsPage()
        {
            InitializeComponent();

            _poolService = App.Container.Resolve<IPoolService>();
            _dialogService = App.Container.Resolve<IDialogService>();

            var ctx = new PoolAndSpaViewModel();
            BindingContext = new PoolAndSpaViewModel()
            {
                IsBusy = true,
            };

            ToolbarItems.Add(new ToolbarItem("Refresh", "ic_refresh_white_48dp.png", async () =>
            {
                await Refresh();
            }));
            ToolbarItems.Add(new ToolbarItem("Save", "ic_save_white_48dp.png", async () =>
            {
                await Save();
            }));

            InitAsync();
        }

        async Task Save()
        {
            var vm = BindingContext as PoolAndSpaViewModel;
            vm.IsBusy = true;

            var savedSchedule = await Task.Run(async () =>
            {
                return await _poolService.SetSchedule(
                    new DateTime(vm.PoolModel.ScheduleStartTime.Ticks),
                    new DateTime(vm.PoolModel.ScheduleEndTime.Ticks),
                    vm.PoolModel.ScheduleIsActive,
                    vm.PoolModel.IncludeBooster);
            });

            if (savedSchedule != null)
            {
                await _dialogService.ShowAlert("Schedule data updated", "");
            }

            vm.IsBusy = false;
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