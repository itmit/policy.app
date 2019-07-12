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
	public partial class AuthPage : ContentPage
	{
		public AuthPage ()
		{
			InitializeComponent ();
		}


        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Login.Text == "admin")
            {
                if (Password.Text == "admin")
                {
                    Application.Current.MainPage = new MainPage();
                    //await Navigation.PushModalAsync(new MainPage());
                }
            }
        }
    }
}