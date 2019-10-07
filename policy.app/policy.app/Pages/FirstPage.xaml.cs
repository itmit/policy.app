using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FirstPage : CarouselPage
	{
		#region .ctor
		public FirstPage()
		{
			InitializeComponent();
		}
		#endregion
	}
}
