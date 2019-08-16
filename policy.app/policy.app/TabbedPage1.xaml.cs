using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using policy.app.Pages;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace policy.app
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TabbedPage1 : TabbedPage
	{
		public TabbedPage1()
		{
			InitializeComponent();
			Effects.Add(new NoShiftEffect());
			On<Android>()
				.SetToolbarPlacement(ToolbarPlacement.Bottom);
		}
	}
}