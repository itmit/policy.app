using System;
using System.Threading.Tasks;
using FreshMvvm;
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

		public UsersListPageModel ViewModel => BindingContext as UsersListPageModel;

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			((UsersListPageModel) BindingContext).SelectedGopher = (sender as View)?.BindingContext as Gopher;
		}

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			((UsersListPageModel)BindingContext).OpenCategory((sender as View)?.BindingContext as Category);
		}


		private void ItemsView_OnScrolled(object sender, ItemsViewScrolledEventArgs e)
		{
			if (ViewModel == null)
			{
				return;
			}
			if (ViewModel.IsBusy)
			{
				return;
			}

			if (e.LastVisibleItemIndex == ViewModel.Users.Count)
			{
				ViewModel.IsBusy = true;

				ViewModel.MoveNext();
			}
		}
	}
}
