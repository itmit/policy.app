using System;
using policy.app.Models;
using policy.app.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsersListPage : ContentPage
	{
		#region .ctor
		public UsersListPage()
		{
			InitializeComponent();
		}
		#endregion

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			((UsersListPageModel) BindingContext).SelectedGopher = (sender as View)?.BindingContext as Gopher;
		}

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			((UsersListPageModel)BindingContext).OpenCategory((sender as View)?.BindingContext as Category);
		}
	}
}
