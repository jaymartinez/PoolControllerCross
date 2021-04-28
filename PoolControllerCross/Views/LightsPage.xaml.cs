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
            var vm = BindingContext as LightsPageViewModel;
            vm.IsBusy = true;

            var savedSchedules = await Task.Run(async () =>
            {
                var pl = await _poolService.SetPoolLightSchedule(
                    new DateTime(vm.PoolLightModel.ScheduleStartTime.Ticks),
                    new DateTime(vm.PoolLightModel.ScheduleEndTime.Ticks),
                    vm.PoolLightModel.ScheduleIsActive);

                var sl = await _poolService.SetSpaLightSchedule(
                    new DateTime(vm.SpaLightModel.ScheduleStartTime.Ticks),
                    new DateTime(vm.SpaLightModel.ScheduleEndTime.Ticks),
                    vm.SpaLightModel.ScheduleIsActive);

                var gl = await _poolService.SetGroundLightSchedule(
                    new DateTime(vm.GroundLightsModel.ScheduleStartTime.Ticks),
                    new DateTime(vm.GroundLightsModel.ScheduleEndTime.Ticks),
                    vm.GroundLightsModel.ScheduleIsActive);

                return new EquipmentSchedule[] { pl, sl, gl };
            });

            if (savedSchedules.Count() == 3)
            {
                await _dialogService.ShowAlert("Schedule data updated", "");
            }
            else
            {
                await _dialogService.ShowAlert("One of the light schedules failed to save, try again.", "");
            }

            vm.IsBusy = false;
        }

        async void InitAsync()
        {
            await Refresh();
        }

        async Task Refresh()
        {
            BindingContext = new LightsPageViewModel()
            {
                IsBusy = true
            };

            var (statuses, groundLightSched, poolLightSched, spaLightSched) = await Task.Run(async () =>
            {
                var status = (await _poolService.GetAllStatuses()).ToList();
                var gLSched = await _poolService.GetGroundLightSchedule();
                var plSched = await _poolService.GetPoolLightSchedule();
                var slSched = await _poolService.GetSpaLightSchedule();

                return (status, gLSched, plSched, slSched);
            });

            var spaLight = statuses.FirstOrDefault(_ => _.PinNumber == Pin.SpaLight);
            var groundLights = statuses.FirstOrDefault(_ => _.PinNumber == Pin.GroundLights);
            var poolLight = statuses.FirstOrDefault(_ => _.PinNumber == Pin.PoolLight);

            var spaLightViewModel = new ScheduleViewModel(spaLightSched, spaLight);
            var groundLightViewModel = new ScheduleViewModel(groundLightSched, groundLights);
            var poolLightViewModel = new ScheduleViewModel(poolLightSched, poolLight);

            BindingContext = new LightsPageViewModel(poolLightViewModel, groundLightViewModel, spaLightViewModel)
            {
                IsBusy = false
            };
        }
    }
}