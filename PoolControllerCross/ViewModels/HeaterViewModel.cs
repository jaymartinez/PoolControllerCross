using eHub.Common.Models;

namespace PoolControllerCross.ViewModels
{
    public class HeaterViewModel : EquipmentViewModel
    {
        public HeaterViewModel(PiPin pin)
            : base(pin) { }
    }
}
