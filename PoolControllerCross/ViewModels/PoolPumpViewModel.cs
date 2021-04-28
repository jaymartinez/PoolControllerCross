using eHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolControllerCross.ViewModels
{
    public class PoolPumpViewModel : ScheduleViewModel
    {
        bool includeBooster;
        public bool IncludeBooster
        {
            get => includeBooster;
            set => SetProperty(ref includeBooster, value, nameof(IncludeBooster));
        }

        public PoolPumpViewModel(PoolSchedule schedule, PiPin pin)
            : base(schedule, pin)
        {
            IncludeBooster = schedule.IncludeBooster;
        }
    }
}
