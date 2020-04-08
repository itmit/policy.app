using policy.app.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RatingPage : ContentPage
	{
		#region .ctor
		public RatingPage()
		{
			InitializeComponent();
		}
		#endregion

		private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (sender is ListView listView)
			{
				listView.SelectedItem = null;
			}
		}

		public RatingPageModel ViewModel => BindingContext as RatingPageModel;

		/// <summary>Application developers can override this method to provide behavior when the back button is pressed.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		protected override bool OnBackButtonPressed() => base.OnBackButtonPressed();

		private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
		{
			if (ViewModel == null)
			{
				return;
			}
			if (ViewModel.IsRefreshing)
			{
				return;
			}

			var y = (int)e.ScrollY;
			if (Gophers.Height - 600 < y)
			{
				ViewModel.MoveNext();
			}
		}
	}
}
