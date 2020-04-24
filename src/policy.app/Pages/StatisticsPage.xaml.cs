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
	public partial class StatisticsPage : ContentPage
	{
		public StatisticsPage()
		{
			InitializeComponent();
			
			var top = 0;
			if (Device.iOS == Device.RuntimePlatform)
			{
				top = 50;
			}

			ImageButton.Margin = new Thickness(10, top, 0, 0);
		}
	}
}