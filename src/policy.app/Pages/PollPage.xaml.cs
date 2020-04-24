using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ListView = Xamarin.Forms.ListView;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PollPage : ContentPage
	{
		#region .ctor
		public PollPage()
		{
			InitializeComponent();
		}
		#endregion
	}
}
