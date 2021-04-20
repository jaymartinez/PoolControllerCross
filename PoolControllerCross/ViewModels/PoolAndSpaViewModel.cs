
namespace PoolControllerCross.ViewModels
{
    public class PoolAndSpaViewModel : BaseViewModel
    {
        public PoolPumpViewModel PoolModel { get; }
        public BoosterPumpViewModel BoosterPumpModel { get; }
        public HeaterViewModel HeaterModel { get; }

        public PoolAndSpaViewModel() { }

        public PoolAndSpaViewModel(PoolPumpViewModel poolModel, 
            BoosterPumpViewModel boosterModel, 
            HeaterViewModel heaterModel)
        {
            PoolModel = poolModel;
            BoosterPumpModel = boosterModel;
            HeaterModel = heaterModel;
        }

    }
}
