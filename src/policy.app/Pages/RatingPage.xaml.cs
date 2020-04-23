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

		public RatingPageModel ViewModel => BindingContext as RatingPageModel;

		private void Gophers_OnScrolled(object sender, ItemsViewScrolledEventArgs e)
		{
			if (ViewModel == null)
			{
				return;
			}
			if (ViewModel.IsBusy)
			{
				return;
			}

			if (e.LastVisibleItemIndex == ViewModel.Gophers.Count)
			{
				ViewModel.IsBusy = true;

				ViewModel.MoveNext();
			}
		}
	}
}
