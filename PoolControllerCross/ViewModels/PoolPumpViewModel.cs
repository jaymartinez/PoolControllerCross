using eHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoolControllerCross.ViewModels
{
    public class PoolPumpViewModel : BaseViewModel
    {
        public PoolPumpViewModel() { }

        PoolSchedule schedule;
        public PoolSchedule Schedule
        {
            get => schedule;
            set => SetProperty(ref schedule, value, nameof(EquipmentSchedule));
        }

        bool poolIsActive;
        public bool PoolIsActive
        {
            get => poolIsActive;
            set => SetProperty(ref poolIsActive, value, nameof(PoolIsActive));
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


        public string ActiveAtText => PoolIsActive && Schedule != null ?
            $"Active at {new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Schedule.StartHour, Schedule.StartMinute, 0).ToShortTimeString()} " : "Off";

        public PoolPumpViewModel(PoolSchedule schedule)
        {
            Schedule = schedule;
            Title = "Pool Pump";
            ScheduleIsActive = schedule.IsActive;
            IncludeBooster = schedule.IncludeBooster;
        }
    }
}
