using eHub.Common.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolControllerCross.ViewModels
{
    public class HeaterViewModel : EquipmentViewModel
    {
        public Command LoadPageCommand { get; }


        public HeaterViewModel(PiPin pin)
            : base(pin) { }

        public HeaterViewModel()
        {
            LoadPageCommand = new Command(async () => await InitializeViewCommand());
        }

        async Task InitializeViewCommand()
        {
            /*
            IsBusy = true;

            try
            {
                var pin = await PoolService.GetPinStatus(Pin.Heater);

                Title = pin.Name;
                if (pin.State == PinState.ON)
                {
                    IsActive = true;
                    StartTime = new TimeSpan(pin.DateActivated.Hour, pin.DateActivated.Minute, 0);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                IsBusy = false;
            }
            */
        }
    }
}
