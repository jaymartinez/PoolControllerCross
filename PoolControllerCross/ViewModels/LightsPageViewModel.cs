using eHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoolControllerCross.ViewModels
{
    public class LightsPageViewModel : BaseViewModel
    {
        public ScheduleViewModel PoolLightModel { get; }
        public ScheduleViewModel GroundLightsModel { get; }
        public ScheduleViewModel SpaLightModel { get; }

        public LightsPageViewModel() { }

        public LightsPageViewModel(
            ScheduleViewModel poolLightModel,
            ScheduleViewModel groundLightsModel,
            ScheduleViewModel spaLightModel)
        {
            PoolLightModel = poolLightModel;
            GroundLightsModel = groundLightsModel;
            SpaLightModel = spaLightModel;
        }
    }
}
