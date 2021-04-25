using eHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolControllerCross.ViewModels
{
    public class PoolPumpViewModel : EquipmentViewModel
    {
        PoolSchedule schedule;
        public PoolSchedule Schedule
        {
            get => schedule;
            set => SetProperty(ref schedule, value, nameof(Schedule));
        }

        public TimeSpan scheduleStartTime;
        public TimeSpan ScheduleStartTime
        {
            get => scheduleStartTime;
            set => SetProperty(ref scheduleStartTime, value, nameof(ScheduleStartTime));
        }

        TimeSpan scheduleEndTime;
        public TimeSpan ScheduleEndTime
        {
            get => scheduleEndTime;
            set => SetProperty(ref scheduleEndTime, value, nameof(ScheduleEndTime));
        }

        bool scheduleIsActive;
        public bool ScheduleIsActive
        {
            get => scheduleIsActive;
            set => SetProperty(ref scheduleIsActive, value, nameof(ScheduleIsActive));
        }

        bool includeBooster;
        public bool IncludeBooster
        {
            get => includeBooster;
            set => SetProperty(ref includeBooster, value, nameof(IncludeBooster));
        }

        public PoolPumpViewModel(PoolSchedule schedule, PiPin pin)
            : base(pin)
        {
            ScheduleIsActive = schedule.IsActive;
            IncludeBooster = schedule.IncludeBooster;
            Schedule = schedule;
            ScheduleStartTime = new TimeSpan(schedule.StartHour, schedule.StartMinute, 0);
            ScheduleEndTime = new TimeSpan(schedule.EndHour, schedule.EndMinute, 0);
        }
    }
}
