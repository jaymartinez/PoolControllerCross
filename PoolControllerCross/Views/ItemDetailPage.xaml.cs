using PoolControllerCross.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PoolControllerCross.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}