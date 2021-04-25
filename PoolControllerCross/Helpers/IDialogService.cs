using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoolControllerCross.Helpers
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmation(string message, string title, Action onDismissCallback, string positiveText = "Yes", string negativeText = "No");
        Task ShowAlert(string title, string message, string okText = "Ok");
    }
}
