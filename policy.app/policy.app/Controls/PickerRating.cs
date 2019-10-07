using Xamarin.Forms;

namespace policy.app.Controls
{
	public class PickerRating : Picker
	{
		#region Data
		#region Static
		public static readonly BindableProperty PlaceholderColorProperty =
			BindableProperty.Create(nameof(PlaceholderColor), typeof(string), typeof(PickerRating), "#19365f", BindingMode.TwoWay);
		#endregion
		#endregion

		#region Properties
		public string PlaceholderColor
		{
			get => (string) GetValue(PlaceholderColorProperty);
			set => SetValue(PlaceholderColorProperty, value);
		}
		#endregion
	}
}
