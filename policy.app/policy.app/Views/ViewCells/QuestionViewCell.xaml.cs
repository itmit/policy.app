using policy.app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace policy.app.Views.ViewCells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestionViewCell : Xamarin.Forms.ViewCell
	{
		public QuestionViewCell()
		{
			InitializeComponent();
		}
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