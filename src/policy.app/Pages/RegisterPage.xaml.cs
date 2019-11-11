using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		#region .ctor
		public RegisterPage()
		{
			InitializeComponent();
		}
		#endregion

		#region Private
		private void Button_OnClicked(object sender, EventArgs e)
		{
			RegisterButton.IsEnabled = true;
		}
		#endregion
	}
}
