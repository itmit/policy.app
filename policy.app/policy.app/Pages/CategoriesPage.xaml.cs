using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriesPage : ContentPage
	{
		#region .ctor
		public CategoriesPage()
		{
			InitializeComponent();
			NativeList.SeparatorVisibility = SeparatorVisibility.None;
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
