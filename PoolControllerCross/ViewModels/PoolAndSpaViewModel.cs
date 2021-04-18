using System;
using System.Collections.Generic;
using System.Text;

namespace PoolControllerCross.ViewModels
{
    public class PoolAndSpaViewModel : BaseViewModel
    {
        public PoolPumpViewModel PoolModel { get; set; }

        bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value, nameof(IsLoading));
        }
    }
}
