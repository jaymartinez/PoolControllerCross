using eHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoolControllerCross.ViewModels
{
    public class ScheduleViewModel : EquipmentViewModel
    {
        public ScheduleViewModel(EquipmentSchedule schedule, PiPin pin)
            : base (pin)
        {
            ScheduleStartTime = new TimeSpan(schedule.StartHour, schedule.StartMinute, 0);
            ScheduleEndTime = new TimeSpan(schedule.EndHour, schedule.EndMinute, 0);
            ScheduleIsActive = schedule.IsActive;
            Schedule = schedule;
        }

        EquipmentSchedule schedule;
        public EquipmentSchedule Schedule
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
    }
}
