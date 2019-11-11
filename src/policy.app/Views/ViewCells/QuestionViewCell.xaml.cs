using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Views.ViewCells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestionViewCell : ViewCell
	{
		#region .ctor
		public QuestionViewCell()
		{
			InitializeComponent();
		}
		#endregion

		/*
		 *
		   private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		   {
		   if (sender is ListView listView)
		   {
		   if (listView.SelectedItem is Answer answer)
		   {
		   answer.IsSelected = true;
		   }
		   listView.SelectedItem = null;
		   }
		   }
		 *
		 */
	}
}
