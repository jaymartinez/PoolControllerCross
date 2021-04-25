
using eHub.Common.Models;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolControllerCross.ViewModels
{
    public class PoolAndSpaViewModel : BaseViewModel
    {
        public PoolPumpViewModel PoolModel { get; private set; }
        public EquipmentViewModel BoosterPumpModel { get; private set; }
        public EquipmentViewModel HeaterModel { get; private set; }

        public PoolAndSpaViewModel() 
        { 
        }

        public PoolAndSpaViewModel(PoolPumpViewModel poolModel, 
            EquipmentViewModel boosterModel, 
            EquipmentViewModel heaterModel)
        {
            PoolModel = poolModel;
            BoosterPumpModel = boosterModel;
            HeaterModel = heaterModel;
        }
    }
}
