using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		#region .ctor
		public LoginPage()
		{
			InitializeComponent();
		}
		#endregion

		private void ImageButton_OnClicked(object sender, EventArgs e)
		{
			PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
		}
	}
}
