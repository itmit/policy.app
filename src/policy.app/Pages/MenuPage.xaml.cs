using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		#region .ctor
		public MenuPage()
		{
			InitializeComponent();
			ll.SeparatorVisibility = SeparatorVisibility.None;
		}
		#endregion

		#region Private
		private void Ll_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			// then reset SelectedItem
			((ListView) sender).SelectedItem = null;
		}
		#endregion
	}
}
