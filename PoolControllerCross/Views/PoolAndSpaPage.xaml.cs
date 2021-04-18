using Autofac;
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
            var model = new PoolAndSpaViewModel();
            model.IsLoading = true;
            BindingContext = model;

            var sched = await _poolService.GetSchedule();

            BindingContext = new PoolAndSpaViewModel()
            {
                IsLoading = false,
                PoolModel = new PoolPumpViewModel(sched)
                {
                    IsActive = sched.IsActive
                }
            };
        }
    }
}