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

        bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value, nameof(IsActive));
        }

        public string ActiveAtText => IsActive && Schedule != null ?
            $"Active at {new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Schedule.StartHour, Schedule.StartMinute, 0).ToShortTimeString()} " : "Off";

        public PoolPumpViewModel(PoolSchedule schedule)
        {
            Schedule = schedule;
            Title = "Pool Pump";
            IsActive = schedule.IsActive;
        }
    }
}
