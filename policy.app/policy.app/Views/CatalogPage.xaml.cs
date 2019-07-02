using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CatalogPage : ContentPage
	{
		public CatalogPage()
		{
			InitializeComponent();
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new MainPage());
		}

		private void Button_OnClicked2(object sender, EventArgs e)
		{
			Navigation.PushAsync(new CatalogPage());
		}
	}
}