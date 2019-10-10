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
	}
}
