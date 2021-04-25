using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoolControllerCross.Helpers
{
    public class DialogService : IDialogService
    {
        public async Task ShowAlert(string title, string message, string okText = "Ok")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, okText);
        }

        public async Task<bool> ShowConfirmation(string message, string title, Action onDismissCallback, string positiveText = "Yes", string negativeText = "No")
        {
            var result = await Application.Current.MainPage.DisplayAlert(title, message, positiveText, negativeText);
            onDismissCallback?.Invoke();
            return result;
        }
    }
}
