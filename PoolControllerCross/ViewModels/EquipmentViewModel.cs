using eHub.Common.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolControllerCross.ViewModels
{
    public class EquipmentViewModel : BaseViewModel
    {
        public PiPin EquipmentPin { get; }
        public int PinNumber { get; }
        
        public int State { get; private set; }

        public Color OnButtonBackgroundColor => IsActive ? Color.LightGreen : Color.Transparent;
        public Color OnButtonTextColor => IsActive ? Color.Black : Color.DarkGreen;

        public Color OffButtonBackgroundColor => IsActive ? Color.Transparent : Color.DarkRed;
        public Color OffButtonTextColor => IsActive ? Color.DarkRed: Color.White;

        public EquipmentViewModel(PiPin pin)
        {
            EquipmentPin = pin;
            PinNumber = pin.PinNumber;
            Title = pin.Name;
            State = pin.State;
            if (pin.State == PinState.ON)
            {
                IsActive = true;
                StartTime = new TimeSpan(pin.DateActivated.Hour, pin.DateActivated.Minute, 0);
            }

            OffCommand = new Command(async (piPin) => await TurnEquipmentOff(piPin));
            OnCommand = new Command(async (piPin) => await TurnEquipmentOn(piPin));
        }

        async Task TurnEquipmentOn(object obj)
        {
            if (!(obj is PiPin pin))
            {
                throw new ArgumentException("Argument is not a PiPin object");
            }

            if (pin.State == PinState.ON)
            {
                return;
            }

            await ToggleEquipment(pin, true);
        }

        async Task TurnEquipmentOff(object obj)
        {
            if (!(obj is PiPin pin))
            {
                throw new ArgumentException("Argument is not a PiPin object");
            }

            if (pin.State == PinState.OFF)
            {
                return;
            }

            await ToggleEquipment(pin, false);
        }

        public Command ToggleCommand { get; }

        public Command OnCommand { get; }
        public Command OffCommand { get; }

        async Task ToggleEquipment(PiPin pin, bool isTurningOn)
        {
            var allStatuses = await Task.Run(async () =>
            {
                return await PoolService.GetAllStatuses();
            });

            if (allStatuses == null || !allStatuses.Any())
            {
                await DialogService.ShowAlert("Error", "Something went wrong when retrieving all equipment statuses from the server");
                return;
            }

            var proceed = true;
            var heaterStatus = allStatuses.FirstOrDefault(_ => _.PinNumber == Pin.Heater);
            var boosterStatus = allStatuses.FirstOrDefault(_ => _.PinNumber == Pin.BoosterPump);
            var spaStatus = allStatuses.FirstOrDefault(_ => _.PinNumber == Pin.SpaPump);
            var curPoolState = allStatuses.FirstOrDefault(_ => _.PinNumber == Pin.PoolPump);

            if (pin.PinNumber == Pin.PoolPump)
            {
                var onOffStr = curPoolState.State == PinState.ON ? "off" : "on";

                if (curPoolState.State == PinState.ON 
                    && (heaterStatus.State == PinState.ON || boosterStatus.State == PinState.ON))
                {
                    await DialogService.ShowAlert("Wait!", "Make sure the heater and booster pump are off first!");
                    return;
                }

                proceed = await DialogService
                    .ShowConfirmation("Are You Sure?", $"Are you sure you want to turn it {onOffStr}?", null);

            }
            else if (pin.PinNumber == Pin.BoosterPump || pin.PinNumber == Pin.Heater)
            {
                var status = allStatuses.FirstOrDefault(_ => _.PinNumber == pin.PinNumber).State;
                if (status == PinState.OFF && curPoolState.State == PinState.OFF)
                {
                    await DialogService.ShowAlert("Wait!", "The pool pump needs to be on first!");
                    return;
                }
            }


            //var confirm = DialogService.ShowConfirmation("Are you sure you want to ")

            if (proceed)
            {
                var result = new PiPin()
                {
                    DateActivated = DateTime.Now,
                    DateDeactivated = DateTime.Now,
                    State = PinState.ON,
                    PinNumber = Pin.BoosterPump
                };
                //var result = await PoolService.Toggle(pin.PinNumber);
                if (result.State == PinState.ON)
                {
                    StartTime = new TimeSpan(result.DateActivated.Hour, result.DateActivated.Minute, 0);
                    State = PinState.ON;
                    IsActive = true;
                }
                else
                {
                    EndTime = new TimeSpan(result.DateDeactivated.Hour, result.DateDeactivated.Minute, 0);
                    State = PinState.OFF;
                    IsActive = false;
                }
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
            set
            {
                SetProperty(ref isActive, value, nameof(IsActive));
                OnPropertyChanged(nameof(ActiveAtText));
                OnPropertyChanged(nameof(OnButtonBackgroundColor));
                OnPropertyChanged(nameof(OnButtonTextColor));
                OnPropertyChanged(nameof(OffButtonBackgroundColor));
                OnPropertyChanged(nameof(OffButtonTextColor));
            }
        }

        TimeSpan? endTime;
        public TimeSpan? EndTime
        {
            get => endTime;
            set => SetProperty(ref endTime, value, nameof(EndTime));
        }

        public string ActiveAtText => IsActive && startTime.HasValue ? 
            $"ON - Active at {startTime.Value.ToString(@"%h\:mm")}" : "OFF";

    }
}
