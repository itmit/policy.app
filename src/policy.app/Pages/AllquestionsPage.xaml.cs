using System;
using policy.app.Models;
using policy.app.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AllQuestionsPage : ContentPage
	{
		#region .ctor
		public AllQuestionsPage()
		{
			InitializeComponent();
		}
		#endregion

		private void TapGestureRecognizer_OnTapped1(object sender, EventArgs e)
		{
			((AllQuestionsPageModel)BindingContext).SelectedPollCategory = ((View)sender).BindingContext as PollCategory;
		}
	}
}
