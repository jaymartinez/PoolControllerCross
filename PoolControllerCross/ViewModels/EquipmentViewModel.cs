using eHub.Common.Models;
using System;

namespace PoolControllerCross.ViewModels
{
    public class EquipmentViewModel : BaseViewModel
    {
        public EquipmentViewModel(PiPin pin)
        {
            Title = pin.Name;
            if (pin.State == PinState.ON)
            {
                IsActive = true;
                StartTime = new TimeSpan(pin.DateActivated.Hour, pin.DateActivated.Minute, 0);
            }
        }

        public TimeSpan? startTime;
        public TimeSpan? StartTime
        {
            get => startTime;
            set => SetProperty(ref startTime, value, nameof(StartTime));
        }

        bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value, nameof(IsActive));
        }

        TimeSpan? endTime;
        public TimeSpan? EndTime
        {
            get => endTime;
            set => SetProperty(ref endTime, value, nameof(EndTime));
        }

        public string ActiveAtText => IsActive && startTime.HasValue ?
            $"Active at {startTime.Value.ToString(@"%h\:mm")}" : "Off";

    }
}
