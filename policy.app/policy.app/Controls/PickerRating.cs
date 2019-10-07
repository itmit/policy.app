using Xamarin.Forms;

namespace policy.app.Controls
{
	/// <summary>
	/// Представляет <see cref="Picker"/>, с возможностью установить цвет placeholder.
	/// </summary>
	public class PickerRating : Picker
	{
		#region Data
		#region Static
		/// <summary>
		/// Свойство цвет placeholder.
		/// </summary>
		public static readonly BindableProperty PlaceholderColorProperty =
			BindableProperty.Create(nameof(PlaceholderColor), typeof(string), typeof(PickerRating), "#19365f", BindingMode.TwoWay);
		#endregion
		#endregion

		#region Properties
		/// <summary>
		/// Возвращает или устанавливает название цвета placeholder.
		/// </summary>
		public string PlaceholderColor
		{
			get => (string) GetValue(PlaceholderColorProperty);
			set => SetValue(PlaceholderColorProperty, value);
		}
		#endregion
	}
}
