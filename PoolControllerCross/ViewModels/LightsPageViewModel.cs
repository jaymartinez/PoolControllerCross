using eHub.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoolControllerCross.ViewModels
{
    public class LightsPageViewModel : BaseViewModel
    {
        public EquipmentViewModel PoolLightModel { get; }
        public EquipmentViewModel GroundLightsModel { get; }
        public EquipmentViewModel SpaLightModel { get; }

    }
}
