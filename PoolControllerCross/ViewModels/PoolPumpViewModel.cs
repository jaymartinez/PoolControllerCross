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
            set => SetProperty(ref schedule, value, nameof(PoolSchedule));
        }

        public TimeSpan startTime;
        public TimeSpan StartTime
        {
            get => startTime;
            set => SetProperty(ref startTime, value, nameof(StartTime));
        }

        TimeSpan endTime;
        public TimeSpan EndTime
        {
            get => endTime;
            set => SetProperty(ref endTime, value, nameof(EndTime));
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
            $"Active at {new TimeSpan(Schedule.StartHour, Schedule.StartMinute, 0):HH:mm} " : "Off";

        public PoolPumpViewModel(PoolSchedule schedule)
        {
            Schedule = schedule;
            Title = "Pool Pump";
            ScheduleIsActive = schedule.IsActive;
            IncludeBooster = schedule.IncludeBooster;
            StartTime = new TimeSpan(schedule.StartHour, schedule.StartMinute, 0);
            EndTime = new TimeSpan(schedule.EndHour, schedule.EndMinute, 0);
        }
    }
}
