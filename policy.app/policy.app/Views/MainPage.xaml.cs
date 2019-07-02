using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new MainPage());
		}

		private void Button_OnClicked2(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				Navigation.PushAsync(new CatalogPage());
			});
		}
	}
}