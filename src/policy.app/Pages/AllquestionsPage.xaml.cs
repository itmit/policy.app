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

		#region Private
		private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView) sender).SelectedItem = null;
		}
		#endregion

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			((AllQuestionsPageModel) BindingContext).SelectedPoll = ((View) sender).BindingContext as Poll;
		}

		private void TapGestureRecognizer_OnTapped1(object sender, EventArgs e)
		{
			((AllQuestionsPageModel)BindingContext).SelectedPollCategory = ((View)sender).BindingContext as PollCategory;
		}
	}
}
