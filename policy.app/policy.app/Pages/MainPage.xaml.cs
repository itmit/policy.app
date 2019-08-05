using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : Xamarin.Forms.TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();

			On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
		}
	}
}