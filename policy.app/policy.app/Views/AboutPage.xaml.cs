using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Views
{
	public partial class AboutPage : ContentPage
	{
		public AboutPage()
		{
			InitializeComponent();
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new MainPage());
		}

		private void Button_OnClicked2(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new CatalogPage());
		}

		private void Button_OnClicked3(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new RatingPage());
		}

		private void Button_OnClicked4(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new AboutPage());
		}
	}
}