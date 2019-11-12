using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ListView = Xamarin.Forms.ListView;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PollPage : ContentPage
	{
		#region .ctor
		public PollPage()
		{
			InitializeComponent();
		}
		#endregion

		#region Private
		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AllQuestionsPage());
		}
		#endregion

		private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView) sender).SelectedItem = null;
		}
	}
}
