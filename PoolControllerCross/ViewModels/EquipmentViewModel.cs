using System;
using System.Collections.Generic;
using System.Text;
using eHub.Common;
using eHub.Common.Models;

namespace PoolControllerCross.ViewModels
{
    public class EquipmentViewModel : BaseViewModel
    {
        EquipmentSchedule schedule;
        public EquipmentSchedule Schedule
        {
            get => schedule;
            set => SetProperty(ref schedule, value, nameof(EquipmentSchedule));
        }

       


        public EquipmentViewModel() { }


    }
}
