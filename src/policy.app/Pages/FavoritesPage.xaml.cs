using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FavoritesPage : ContentPage
	{
		#region .ctor
		public FavoritesPage()
		{
			InitializeComponent();
		}
		#endregion

		#region Private
		private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView) sender).SelectedItem = null;
		}
		#endregion
	}
}
