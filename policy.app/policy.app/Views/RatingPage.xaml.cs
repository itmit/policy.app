using policy.app.Pages;
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
	public partial class RatingPage : ContentPage
	{
		public RatingPage()
		{
			InitializeComponent();
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new HomePage());
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