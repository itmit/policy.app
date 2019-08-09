using Xamarin.Forms;

namespace policy.app.Pages
{
	public class BackTabPage : ContentPage
	{
		public BackTabPage()
		{
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Welcome to Xamarin.Forms!" }
				}
			};
		}
	}
}