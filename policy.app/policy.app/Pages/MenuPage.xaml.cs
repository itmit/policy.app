using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		public MenuPage()
		{
			InitializeComponent();
		}

        private void Ll_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // then reset SelectedItem
            ((ListView)sender).SelectedItem = null;
        }
    }
}